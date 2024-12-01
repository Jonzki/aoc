namespace Aoc.Problems.Aoc20
{
    public class Problem7 : IProblem
    {
        public object Solve1(string input)
        {
            // Read the bag rules.
            var bagRules = input
                .Split(Environment.NewLine).Select(ParseBagRule)
                .ToArray();

            // Find all bags that can contain a "shiny gold" bag.
            const string targetColor = "shiny gold";
            var validBags = new HashSet<string>();

            int count = 0, loops = 0;
            do
            {
                ++loops;
                count = validBags.Count;
                foreach (var rule in bagRules)
                {
                    // Find a bag that either has the target color directly or contains a bag that we know contains the target.
                    var bagColor = rule.Contents.FirstOrDefault(x => x.Color == targetColor || validBags.Contains(x.Color)).Color;
                    if (bagColor != null)
                    {
                        validBags.Add(rule.Color);
                    }
                }
            } while (count != validBags.Count); // Stop when there's no more changes.

            // How many bag colors can eventually contain at least one shiny gold bag?
            return validBags.Count;
        }

        public object Solve2(string input)
        {
            // Read the bag rules.
            var bagRules = input
                .Split(Environment.NewLine).Select(ParseBagRule)
                .ToArray();

            // Find all bags that can contain a "shiny gold" bag.
            const string targetColor = "shiny gold";
            var validBags = new HashSet<string>();

            // Store bag counts for each bag color. -1 by default ("not calculated").
            var childBagCounts = bagRules.SelectMany(r => r.Contents.Select(c => c.Color).Append(r.Color)).Distinct().ToDictionary(c => c, c => -1);

            // Loop through the rules until we have made no more modifications.
            bool modified = false;
            int loops = 0;
            do
            {
                ++loops;
                modified = false;
                foreach (var rule in bagRules)
                {
                    var color = rule.Color;

                    var childBagCount = childBagCounts[color];

                    // If child bag count is not -1, we've already calculated this color.
                    if (childBagCount != -1) continue;

                    // Attempt to calculate the child bag count.
                    var count = 0;
                    foreach (var c in rule.Contents)
                    {
                        // If a rule contains the target color, skip it (somewhere else in the hierarchy).
                        if(c.Color == targetColor)
                        {
                            count = -1;
                            break;
                        }

                        var childCount = childBagCounts[c.Color];
                        if (childCount == -1)
                        {
                            // Some child has not been calculated yet - skip on this loop.
                            count = -1;
                            break;
                        }
                        // Add to the count.
                        count += c.Quantity * (1 + childCount);
                    }

                    if (count != -1)
                    {
                        childBagCounts[color] = count;
                        modified = true;
                    }
                }
            } while (modified); // Stop when there's no more changes.

            // Find the child bag count for the shiny gold bag.
            var targetCount = childBagCounts[targetColor];
            return targetCount;
        }

        public static BagRule ParseBagRule(string input)
        {
            var parts = input.Replace(".", "").Replace("bags", "").Replace("bag", "").Split("contain");

            var rule = new BagRule
            {
                Color = parts[0].Trim(),
                Contents = new List<(string Color, int Quantity)>()
            };

            // Handle "no other bags". Remember, we've removed the "bags" word earlier.
            if (parts[1].Trim() == "no other") return rule;

            foreach (var contentPart in parts[1].Split(','))
            {
                var temp = contentPart.Trim().Split(' ', 2);
                var quantity = int.Parse(temp[0]);
                var color = temp[1].Trim();
                rule.Contents.Add((color, quantity));
            }

            return rule;
        }

        public record BagRule
        {
            public string Color { get; set; }

            public List<(string Color, int Quantity)> Contents { get; set; }

            public override string ToString()
            {
                var sb = new StringBuilder();
                sb.Append(Color);
                sb.Append(" bags contain ");

                if (Contents.Count == 0)
                {
                    sb.Append("no other bags");
                }
                else
                {
                    sb.Append(string.Join(", ", Contents.Select(bag =>
                        $"{bag.Quantity} {bag.Color} bag" + (bag.Quantity > 1 ? "s" : "")
                    )));
                }

                sb.Append(".");

                return sb.ToString();
            }
        }
    }
}
