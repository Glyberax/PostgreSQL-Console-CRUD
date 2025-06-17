// Program.cs
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PostgreSQLConsoleApp
{
    // Model - Kullanıcı tablosu
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; }
        
        public int Age { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }

    // DbContext - Veritabanı bağlantısı
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=TestDB;Username=postgres;Password=4512");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Index Maile index oluşturuyoruz aramak için
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }

    // CRUD İşlemleri
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        // CREATE - Kullanıcı ekleme
        public async Task<User> CreateUserAsync(string name, string email, int age)
        {
            var user = new User
            {
                Name = name,
                Email = email,
                Age = age
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            Console.WriteLine($"✅ Kullanıcı eklendi: {user.Name} (ID: {user.Id})");
            return user;
        }

        // READ - Tüm kullanıcıları listeleme
        public async Task<List<User>> GetAllUsersAsync()
        {
            var users = await _context.Users.OrderBy(u => u.Id).ToListAsync();
            
            Console.WriteLine("\n📋 Tüm Kullanıcılar:");
            Console.WriteLine("ID | İsim           | Email                    | Yaş | Oluşturma Tarihi");
            Console.WriteLine("---|----------------|--------------------------|-----|------------------");
            
            foreach (var user in users)
            {
                Console.WriteLine($"{user.Id,2} | {user.Name,-14} | {user.Email,-24} | {user.Age,3} | {user.CreatedDate:dd.MM.yyyy HH:mm}");
            }
            
            return users;
        }

        // READ - ID ile kullanıcı bulma
        public async Task<User?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            
            if (user != null)
            {
                Console.WriteLine($"👤 Kullanıcı bulundu: {user.Name} - {user.Email}");
            }
            else
            {
                Console.WriteLine($"❌ ID {id} ile kullanıcı bulunamadı");
            }
            
            return user;
        }

        // UPDATE - Kullanıcı güncelleme
        public async Task<bool> UpdateUserAsync(int id, string? name = null, string? email = null, int? age = null)
        {
            var user = await _context.Users.FindAsync(id);
            
            if (user == null)
            {
                Console.WriteLine($"❌ ID {id} ile kullanıcı bulunamadı");
                return false;
            }

            if (!string.IsNullOrEmpty(name)) user.Name = name;
            if (!string.IsNullOrEmpty(email)) user.Email = email;
            if (age.HasValue) user.Age = age.Value;

            await _context.SaveChangesAsync();
            Console.WriteLine($"✅ Kullanıcı güncellendi: {user.Name}");
            
            return true;
        }

        // DELETE - Kullanıcı silme
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            
            if (user == null)
            {
                Console.WriteLine($"❌ ID {id} ile kullanıcı bulunamadı");
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            Console.WriteLine($"🗑️ Kullanıcı silindi: {user.Name}");
            
            return true;
        }
    }

    // Ana Program
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("🚀 PostgreSQL Console CRUD Uygulaması");
            Console.WriteLine("=====================================\n");

            using var context = new AppDbContext();
            var userService = new UserService(context);

            try
            {
                // Veritabanı oluşturma (Code First Migration)
                Console.WriteLine("📦 Veritabanı oluşturuluyor...");
                await context.Database.EnsureCreatedAsync();
                Console.WriteLine("✅ Veritabanı hazır!\n");

                // Direkt interaktif menüye git
                await InteractiveMenu(userService);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Hata: {ex.Message}");
            }
        }

        static async Task DemoOperations(UserService userService)
        {
            Console.WriteLine("🎯 Demo İşlemleri Başlıyor...\n");

            // CREATE - Sadece Arda'yı ekleme
            var addedUser = await userService.CreateUserAsync("Arda Çalışkan", "alkanlardal@icloud.com", 18);

            // READ - Tüm kullanıcıları listeleme
            await userService.GetAllUsersAsync();

            // READ - ID ile arama (eklenen kullanıcının ID'si ile)
            Console.WriteLine("\n🔍 ID ile arama:");
            await userService.GetUserByIdAsync(addedUser.Id);

            // UPDATE - Kullanıcı güncelleme
            Console.WriteLine("\n📝 Güncelleme işlemi:");
            await userService.UpdateUserAsync(addedUser.Id, name: "Arda Çalışkan (Güncellenmiş)", age: 19);

            // Güncellenmiş listeyi göster
            await userService.GetAllUsersAsync();

            Console.WriteLine("\n" + new string('=', 50));
        }

        static async Task InteractiveMenu(UserService userService)
        {
            while (true)
            {
                Console.WriteLine("\n📋 İŞLEM MENÜSÜ:");
                Console.WriteLine("1. Kullanıcı Ekle");
                Console.WriteLine("2. Tüm Kullanıcıları Listele");
                Console.WriteLine("3. Kullanıcı Ara (ID)");
                Console.WriteLine("4. Kullanıcı Güncelle");
                Console.WriteLine("5. Kullanıcı Sil");
                Console.WriteLine("0. Çıkış");
                Console.Write("\nSeçiminiz: ");

                var secim = Console.ReadLine();

                try
                {
                    switch (secim)
                    {
                        case "1":
                            await AddUserInteractive(userService);
                            break;
                        case "2":
                            await userService.GetAllUsersAsync();
                            break;
                        case "3":
                            await SearchUserInteractive(userService);
                            break;
                        case "4":
                            await UpdateUserInteractive(userService);
                            break;
                        case "5":
                            await DeleteUserInteractive(userService);
                            break;
                        case "0":
                            Console.WriteLine("👋 Görüşmek üzere!");
                            return;
                        default:
                            Console.WriteLine("❌ Geçersiz seçim!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Hata: {ex.Message}");
                }
            }
        }

        static async Task AddUserInteractive(UserService userService)
        {
            Console.Write("İsim: ");
            var name = Console.ReadLine();
            
            Console.Write("Email: ");
            var email = Console.ReadLine();
            
            Console.Write("Yaş: ");
            if (int.TryParse(Console.ReadLine(), out int age))
            {
                await userService.CreateUserAsync(name!, email!, age);
            }
            else
            {
                Console.WriteLine("❌ Geçersiz yaş!");
            }
        }

        static async Task SearchUserInteractive(UserService userService)
        {
            Console.Write("Aranacak kullanıcı ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                await userService.GetUserByIdAsync(id);
            }
            else
            {
                Console.WriteLine("❌ Geçersiz ID!");
            }
        }

        static async Task UpdateUserInteractive(UserService userService)
        {
            Console.Write("Güncellenecek kullanıcı ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Console.Write("Yeni isim (boş bırakın değişmesin): ");
                var name = Console.ReadLine();
                
                Console.Write("Yeni email (boş bırakın değişmesin): ");
                var email = Console.ReadLine();
                
                Console.Write("Yeni yaş (boş bırakın değişmesin): ");
                var ageStr = Console.ReadLine();
                int? age = string.IsNullOrEmpty(ageStr) ? null : int.Parse(ageStr);

                await userService.UpdateUserAsync(id, 
                    string.IsNullOrEmpty(name) ? null : name,
                    string.IsNullOrEmpty(email) ? null : email,
                    age);
            }
            else
            {
                Console.WriteLine("❌ Geçersiz ID!");
            }
        }

        static async Task DeleteUserInteractive(UserService userService)
        {
            Console.Write("Silinecek kullanıcı ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Console.Write($"ID {id}'li kullanıcıyı silmek istediğinizden emin misiniz? (e/h): ");
                var confirm = Console.ReadLine()?.ToLower();
                
                if (confirm == "e" || confirm == "evet")
                {
                    await userService.DeleteUserAsync(id);
                }
                else
                {
                    Console.WriteLine("İşlem iptal edildi.");
                }
            }
            else
            {
                Console.WriteLine("❌ Geçersiz ID!");
            }
        }
    }
}