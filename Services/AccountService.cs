using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectManager.Data;
using ProjectManager.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
#pragma warning disable

namespace ProjectManager.Services
{
    public class AccountService : IAccountService
    {
        private IConfiguration config;
        readonly ProjectContext context;
        private ICryptoService crypt;
        public AccountService(IConfiguration config, ProjectContext context, ICryptoService crypt)
        {
            this.config = config;
            this.context = context;
            this.crypt = crypt;
        }

        public async Task<Account> GetAccountAsync(string token)
        {
            var client = await context.Account.FirstOrDefaultAsync(u => u.Token.CompareTo(token) == 0);
            if (client != null) return client;
            return null;
        }

        public async Task<Profile> GetProfileAsync(int id)
        {
            var profile = await context.Profile.FirstOrDefaultAsync(p => p.AccountId == id);
            return profile;
        }

        public async Task<Token?> CreateAccountAsync(AccountModel account)
        {
            var client = await context.Account.FirstOrDefaultAsync(u =>
                u.Username.CompareTo(account.username) == 0 ||
                u.Address.CompareTo(account.address) == 0);

            if (client != null) return null;
            var ms = new MemoryStream();

            client = new Account
            {
                Username = account.username,
                Password = crypt.Encrypt(account.password),
                Address = account.address
            };

            context.Account.Add(client);
            await context.SaveChangesAsync();
            string token = GenToken(client);

            client.Token = token;
            await context.SaveChangesAsync();

            if (!string.IsNullOrEmpty(account.profile.FileName))
            {
                var profile = await context.Profile.FirstOrDefaultAsync(img =>
                    img.Filename.CompareTo(account.profile.FileName) == 0);

                await account.profile.CopyToAsync(ms);

                profile = new Profile
                {
                    Filename = account.profile.FileName,
                    ContentType = account.profile.ContentType,
                    Buffer = ms.ToArray(),
                    Status = true,
                    AccountId = client.Id
                };

                context.Profile.Add(profile);
                await context.SaveChangesAsync();
            }
            else
            {
                byte[] data = File.ReadAllBytes("/profile.png");
                await ms.WriteAsync(data, 0, data.Length);

                var profile = new Profile
                {
                    Filename = "profile.png",
                    ContentType = "image/png",
                    Buffer = ms.ToArray(),
                    AccountId = client.Id
                };

                context.Profile.Add(profile);
                await context.SaveChangesAsync();
            }

            return new Token
            { 
                AccessToken = token
            };
        }

        public async Task<int> CreateProfileAsync(string token, IFormFile img)
        {
            var client = await context.Account.FirstOrDefaultAsync(u => u.Token == token);

            if (client != null)
            {
                var profile = await context.Profile.FirstOrDefaultAsync(p => p.AccountId == client.Id);
                var ms = new MemoryStream();
                await img.CopyToAsync(ms);

                if (profile != null && !string.IsNullOrEmpty(img.FileName) && CanUpdate(profile.Buffer, ms.ToArray()))
                {
                    profile.Filename = img.FileName;
                    profile.ContentType = img.ContentType;
                    profile.Status = true;

                    profile.Buffer = ms.ToArray();
                    await context.SaveChangesAsync();
                    return 2;
                }
                else
                {
                    if (string.IsNullOrEmpty(img.FileName))
                    {
                        byte[] data = File.ReadAllBytes("/profile.png");
                        await ms.WriteAsync(data, 0, data.Length);

                        profile = new Profile
                        {
                            Filename = "profile.png",
                            ContentType = "image/png",
                            Buffer = ms.ToArray(),
                            AccountId = client.Id
                        };

                        context.Profile.Add(profile);
                        await context.SaveChangesAsync();
                        return 0;
                    }

                    profile = new Profile
                    {
                        Filename = img.FileName,
                        ContentType = img.ContentType,
                        Buffer = ms.ToArray(),
                        AccountId = client.Id,
                        Status = true
                    };

                    context.Profile.Add(profile);
                    await context.SaveChangesAsync();
                    return 1;
                }
            }

            return -1;
        }

        public async Task<(Token?, int)> SignIn(string address, string password)
        {
            var client = await context.Account.FirstOrDefaultAsync(u => u.Address.CompareTo(address) == 0);

            if(client != null)
            {
                var key = crypt.Encrypt(password);
                if (client.Password.CompareTo(key) != 0) return (null, -1);

                var token = new Token
                {
                    AccessToken = GenToken(client)
                };

                client.Token = token.AccessToken;
                await context.SaveChangesAsync();
                return (token, 1);
            }

            return (null, 0);
        }

        private string GenToken(Account account)
        {
            var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(config["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(config["JwtSettings:Issuer"],
              config["JwtSettings:Audience"],
              new Claim[] {
                  new Claim("email", account.Address),
                  new Claim("name", account.Username)
              },
              expires: DateTime.Now.AddMinutes(2),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<int> DeleteProfileAsync(string token)
        {
            var client = await context.Account.FirstOrDefaultAsync(u => u.Token == token);
            if (client == null) return -1;

            var img = await context.Profile.FirstOrDefaultAsync(p => p.AccountId == client.Id);
            if (!img.Status) return 0;

            context.Profile.Remove(img);
            await context.SaveChangesAsync();
            return 1;
        }

        public async Task<bool> SignOut(string token)
        {
            var client = await context.Account.FirstOrDefaultAsync(u => u.Token.CompareTo(token) == 0);

            if(client != null)
            {
                client.Token = null;
                await context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<Token> RefreshTokenAsync(string token)
        {
            var client = context.Account.FirstOrDefault(u => u.Token.CompareTo(token) == 0);
            if(client != null)
            {
                token = GenToken(client);
                client.Token = token;
                await context.SaveChangesAsync();

                var access = new Token();
                access.AccessToken = token;
                return access;
            }

            return null;
        }

        private bool CanUpdate(byte[] left, byte[] right)
        {
            if (left.Length != right.Length) return true;
            byte[] a, b;

            using(var hash = MD5.Create())
            {
                a = hash.ComputeHash(left);
                b = hash.ComputeHash(right);
            }

            for (int i = 0; i < a.Length; i++)
                if (a[i] != b[i]) return true;
            return false;
        }

        public async Task<int> UpdateProfileAsync(string token, IFormFile img)
        {
            var client = await context.Account.FirstOrDefaultAsync(u => u.Token.CompareTo(token) == 0);

            if (client != null)
            {
                var profile = await context.Profile.FirstOrDefaultAsync(p => p.AccountId == client.Id);
                var ms = new MemoryStream();

                if (string.IsNullOrEmpty(img.FileName)) return 0;
                if (profile != null)
                {
                    await img.CopyToAsync(ms);

                    if (img.FileName.CompareTo(profile.Filename) != 0 && CanUpdate(profile.Buffer, ms.ToArray()))
                    { 
                        profile.Filename = img.FileName;
                        profile.ContentType = img.ContentType;

                        profile.Buffer = ms.ToArray();
                        await context.SaveChangesAsync();
                        return 2;
                    }

                    return 0;
                }
                else
                {
                    profile = new Profile
                    {
                        Filename = img.FileName,
                        ContentType = img.ContentType,
                        Buffer = ms.ToArray(),
                        Status = true
                    };

                    context.Profile.Add(profile);
                    await context.SaveChangesAsync();
                    return 1;
                }
            }

            return -1;
        }
    }
}
