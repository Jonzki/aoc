using Aoc.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aoc.Problems.Aoc20
{
    public class Problem4 : IProblem
    {
        private static readonly string[] ValidEyeColors = new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

        public object Solve1(string input)
        {
            var passports = input.Split(Environment.NewLine + Environment.NewLine).Select(ParsePassport).ToArray();

            var valid = 0;
            foreach (var passport in passports)
            {
                if (IsPasswordValid1(passport)) ++valid;
            }

            return valid;
        }

        public object Solve2(string input)
        {
            var passports = input.Split(Environment.NewLine + Environment.NewLine).Select(ParsePassport).ToArray();

            var valid = 0;
            foreach (var passport in passports)
            {
                if (IsPasswordValid2(passport))
                {
                    ++valid;
                }
            }

            return valid;
        }


        public static Passport ParsePassport(string input)
        {
            var passport = new Passport();

            var fields = input.Replace("\r", "").Replace("\n", " ").Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var field in fields)
            {
                var parts = field.Split(':', 2);
                passport.Data.AddOrUpdate(parts[0].Trim(), parts[1].Trim());
            }

            return passport;
        }

        public static bool IsPasswordValid1(Passport passport)
        {
            // 7 or 8 members of data need to be present.
            if (passport.Data.Count != 7 && passport.Data.Count != 8) return false;

            // If 7 members, cid should not be there (optional).
            // This means some required member is missing.
            if (passport.Data.Count == 7 && passport.Data.ContainsKey("cid")) return false;

            return true;
        }

        public static bool IsPasswordValid2(Passport passport)
        {
            // Use step 1 to validate the members being present.
            if (!IsPasswordValid1(passport)) return false;

            foreach (var kvp in passport.Data)
            {
                if (!ValidateField(kvp.Key, kvp.Value)) return false;
            }
            return true;
        }

        public static bool ValidateField(string field, string value)
        {
            long number;
            if (!long.TryParse(value, out number)) number = -1;

            bool ValidateHeight()
            {
                int temp = -1;
                if (value.EndsWith("cm") && int.TryParse(value.Substring(0, value.Length - 2), out temp))
                {
                    // If cm, the number must be at least 150 and at most 193.
                    return temp.BetweenInclusive(150, 193);
                }
                if (value.EndsWith("in") && int.TryParse(value.Substring(0, value.Length - 2), out temp))
                {
                    // If in, the number must be at least 59 and at most 76.
                    return temp.BetweenInclusive(59, 76);
                }
                return false;
            }

            return field switch
            {
                // byr(Birth Year) - four digits; at least 1920 and at most 2002.
                "byr" => number.BetweenInclusive(1920, 2002),

                // iyr(Issue Year) - four digits; at least 2010 and at most 2020.
                "iyr" => number.BetweenInclusive(2010, 2020),

                // eyr(Expiration Year) - four digits; at least 2020 and at most 2030.
                "eyr" => number.BetweenInclusive(2020, 2030),

                // hgt(Height) - a number followed by either cm or in:
                "hgt" => ValidateHeight(),

                // hcl(Hair Color) - a # followed by exactly six characters 0-9 or a-f.
                "hcl" => Regex.IsMatch(value, @"^#[0-9a-f]{6}$"),

                // ecl(Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
                "ecl" => ValidEyeColors.Contains(value),

                // pid(Passport ID) - a nine-digit number, including leading zeroes.
                "pid" => Regex.IsMatch(value, "^[0-9]{9}$"),

                // cid(Country ID) - ignored, missing or not.
                "cid" => true,

                _ => throw new InvalidOperationException($"Unexpected field '{field}'.")
            };
        }

        public record Passport
        {
            public Dictionary<string, string> Data { get; } = new Dictionary<string, string>();

            public string BYR => Data.TryGetValue("byr", out var v) ? v : null;
            public string IYR => Data.TryGetValue("iyr", out var v) ? v : null;
            public string EYR => Data.TryGetValue("eyr", out var v) ? v : null;
            public string HGT => Data.TryGetValue("hgt", out var v) ? v : null;
            public string HCL => Data.TryGetValue("hcl", out var v) ? v : null;
            public string ECL => Data.TryGetValue("ecl", out var v) ? v : null;
            public string PID => Data.TryGetValue("pid", out var v) ? v : null;
            public string CID => Data.TryGetValue("cid", out var v) ? v : null;
        }
    }
}
