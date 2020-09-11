using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Xunit;

namespace Microsoft.EntityFrameworkCore
{
    public class RegexIsMatchTest : IClassFixture<RegexIsMatchFixture>
    {
        public RegexIsMatchTest(RegexIsMatchFixture fixture)
            => Fixture = fixture;
        protected RegexIsMatchFixture Fixture { get; }

        [ConditionalFact]
        public void Can_use_Regex_IsMatch()
        {
            using (var context = CreateContext())
            {
                var honeyDijon = context.Add(new User { Name = "Test1" }).Entity;
                var buffaloBleu = context.Add(new User { Name = "NTest2" }).Entity;
                var buffaloBleuNull = context.Add(new User { Name = null }).Entity;

                context.SaveChanges();

                var startingWithT = context.Users.Where(f => Regex.IsMatch(f.Name, @"^T"));

                var nullPattern = context.Users.Where(f => Regex.IsMatch(f.Name, null));

                Assert.Single(startingWithT);
                Assert.Empty(nullPattern);
            }
        }

        private RegexIsMatchContext CreateContext()
            => Fixture.CreateContext();
    }
    public class RegexIsMatchFixture : SharedStoreFixtureBase<RegexIsMatchContext>
    {
        protected override string StoreName { get; } = "RegexIsMatchTest";

        protected override ITestStoreFactory TestStoreFactory
            => SqliteTestStoreFactory.Instance;

        protected override Type ContextType
            => typeof(RegexIsMatchContext);

        public override DbContextOptionsBuilder AddOptions(DbContextOptionsBuilder builder)
        {
            return base.AddOptions(builder).UseSqlite(f => f.EnableRegex());
        }
    }

    public class RegexIsMatchContext : PoolableDbContext
    {
        public RegexIsMatchContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
