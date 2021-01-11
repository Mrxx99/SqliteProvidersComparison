using System;
using System.Collections.Generic;
using Bogus;

namespace Shared
{
    public static class DataGenerator
    {
        public static IEnumerable<User> GenerateUsers(int count)
        {
            Randomizer.Seed = new Random(8675309);

            var addressFaker = new Faker<Address>()
                .RuleFor(o => o.City, f => f.Address.City());

            var addresses = addressFaker.Generate(count / 2 + 1);

            var userFaker = new Faker<User>()
                .RuleFor(o => o.Name, f => f.Name.FullName())
                .RuleFor(o => o.Address, f => f.PickRandom(addresses));

            return userFaker.Generate(count);
        }
    }
}
