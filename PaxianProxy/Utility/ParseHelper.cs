namespace StatHarvester.Utility
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Text.RegularExpressions;
    using DAL.Models;

    public static class ParseHelper
    {
        public static Item ParseItem(string message)
        {
            var lines = message.Split(new[] {"\r\n", "\n"}, StringSplitOptions.RemoveEmptyEntries);
            var name = GetName(lines);
            var (tech, rarity, type) = GetTechRarityAndType(lines);
            var (description, descEndLine) = GetDescription(lines);
            var structuredInfo = GetStructuredInfo(lines, descEndLine);
            var weight = GetWeight(structuredInfo);
            var size = GetSize(structuredInfo);

            return new Item
            {
                Name = name,
                Description = description,
                Tech = tech,
                Rarity = rarity,
                Type = type,
                Size = size,
                Weight = weight,
                StructuredDescription = JsonSerializer.Serialize(structuredInfo)
            };
        }

        private static (string Description, int DescriptionEndLine) GetDescription(string[] lines)
        {
            var startIdx = Array.IndexOf(lines, lines.First(l => l.StartsWith("Most profitable ")));
            for (var i = startIdx + 1; i < lines.Length; i++)
            {
                if (lines[i].Length <= 0 || lines[i][0] == ' ')
                {
                    continue;
                }

                i++;
                var descriptionStartIdx = i;
                for (i++; i < lines.Length; i++)
                {
                    if (Regex.IsMatch(lines[i], @"^[\w|\s]*:.*"))
                    {
                        break;
                    }
                }

                return (lines.Skip(descriptionStartIdx).Take(i - descriptionStartIdx).Aggregate((a, b) => $"{a}\n{b}"),
                    i);
            }

            throw new InvalidDataException();
        }

        private static long GetSize(Dictionary<string, object> structuredFields)
        {
            if (structuredFields.TryGetValue("Weight", out var weight))
            {
                structuredFields.Remove("Weight");
                return long.Parse(((string) weight).Replace(",", string.Empty));
            }

            return 0;
        }

        private static long GetWeight(IDictionary<string, object> structuredFields)
        {
            if (structuredFields.TryGetValue("Size", out var size))
            {
                structuredFields.Remove("Size");
                return long.Parse(((string) size).Replace(",", string.Empty));
            }

            return 0;
        }

        private static Dictionary<string, object> GetStructuredInfo(string[] lines, int descEndLine)
        {
            var baseItemFields = new[]
            {
                "Input:",
                "Inputs:",
                "Outputs:",
                "Output:",
                "Initial Materials:",
                "Periodic Materials:",
                "Built in items:"
            };

            var augStatFields = new[]
            {
                "Improves:"
            };

            var resistsFields = new[]
            {
                "Resists:",
                "Vulnerabilities:"
            };

            var countFields = new[]
            {
                "Max augmenters after upgrade:"
            };

            var stringArrayFields = new[]
            {
                "Actions:",
                "Fields Generated:"
            };

            var fields = new Dictionary<string, object>();
            var requiresFields = new List<int>();
            for (var i = descEndLine; i < lines.Length; ++i)
            {
                if (Regex.IsMatch(lines[i], @"^Requires.*\d+\sin.*"))
                {
                    requiresFields.Add(i);
                    continue;
                }

                if (!lines[i].Contains(":") || lines[i] == "Ship stats:")
                {
                    continue;
                }

                if (baseItemFields.Any(f => lines[i].StartsWith(f)))
                {
                    var originalIdx = i;
                    fields[lines[originalIdx].Replace(":", string.Empty).Trim()] = ReadBaseItems(lines, ref i);
                }
                else if (augStatFields.Any(f => lines[i].StartsWith(f)))
                {
                    var originalIdx = i;
                    fields[lines[originalIdx].Replace(":", string.Empty).Trim()] = ReadAugStats(lines, ref i);
                }
                else if (resistsFields.Any(f => lines[i].StartsWith(f)))
                {
                    var originalIdx = i;
                    fields[lines[originalIdx].Replace(":", string.Empty).Trim()] = ReadResists(lines, ref i);
                }
                else if (countFields.Any(f => lines[i].StartsWith(f)))
                {
                    var originalIdx = i;
                    fields[lines[originalIdx].Replace(":", string.Empty).Trim()] = ReadCountFields(lines, ref i);
                }
                else if (stringArrayFields.Any(f => lines[i].StartsWith(f)))
                {
                    var originalIdx = i;
                    fields[lines[originalIdx].Replace(":", string.Empty).Trim()] = ReadStringArray(lines, ref i);
                }
                else
                {
                    var splitIdx = lines[i].IndexOf(":", StringComparison.Ordinal);
                    var key = lines[i].Substring(0, splitIdx);
                    var value = lines[i].Substring(splitIdx + 2, lines[i].Length - splitIdx - 2);
                    fields[key] = value.Replace("\r", string.Empty);
                }
            }

            if (requiresFields.Any())
            {
                fields["Requires"] = requiresFields
                    .Select(requireIdx =>
                        new CountField
                        {
                            Item = Regex.Match(lines[requireIdx], @"(?<=^Requires\s\d+\sin\s).*").Value,
                            Count = long.Parse(Regex.Match(lines[requireIdx], @"(?<=^Requires\s)\d+").Value)
                        }).ToArray();
            }

            return fields;
        }

        private static List<string> ReadStringArray(string[] lines, ref int i)
        {
            var items = new List<string>();
            for (i++; i < lines.Length; ++i)
            {
                if (!lines[i].StartsWith(' ') && !lines[i].StartsWith('\t'))
                {
                    break;
                }

                var item = Regex.Match(lines[i], @"(?<=[\s|\t]*)[a-zA-Z0-9_.-].*").Value;
                items.Add(item);
            }

            i--;
            return items;
        }

        private static List<CountField> ReadBaseItems(string[] lines, ref int i)
        {
            var items = new List<CountField>();
            for (i++; i < lines.Length; ++i)
            {
                if (!lines[i].StartsWith(' '))
                {
                    break;
                }

                var countStr = Regex.Match(lines[i], @"(?<=\s+)[\d|,]+").Value;
                if (countStr != string.Empty)
                {
                    items.Add(new CountField
                    {
                        Item = Regex.Match(lines[i], @"(?<=\s*[\d|,]+\s).*").Value,
                        Count = long.Parse(countStr.Replace(",", string.Empty))
                    });
                }
                else
                {
                    items.Add(new CountField
                    {
                        Item = lines[i].Trim(),
                        Count = 1
                    });
                }
            }

            i--;
            return items;
        }

        private static List<CountField> ReadCountFields(string[] lines, ref int i)
        {
            var items = new List<CountField>();
            for (i++; i < lines.Length; ++i)
            {
                if (!lines[i].StartsWith(' '))
                {
                    break;
                }

                var parts = lines[i].Split(":");
                var name = parts[0].Trim();
                var count = long.Parse(parts[1].Trim());
                items.Add(new CountField
                {
                    Item = name,
                    Count = count
                });
            }

            i--;
            return items;
        }

        private static List<AugStat> ReadAugStats(string[] lines, ref int i)
        {
            var items = new List<AugStat>();
            for (i++; i < lines.Length; ++i)
            {
                if (!lines[i].StartsWith(' '))
                {
                    break;
                }

                var name = Regex.Match(lines[i], @"(?<=\s*).*(?=\s[+|-])").Value.Trim();
                var value = double.Parse(Regex.Match(lines[i], @"(?<=.*\s[+|-])\d*").Value.Replace(",", string.Empty));
                var isFlat = !lines[i].EndsWith("%");
                items.Add(new AugStat
                {
                    Stat = name,
                    Amount = value,
                    IsFlat = isFlat
                });
            }

            i--;
            return items;
        }

        private static List<CountField> ReadResists(string[] lines, ref int i)
        {
            var items = new List<CountField>();
            for (i++; i < lines.Length; ++i)
            {
                if (!lines[i].StartsWith(' '))
                {
                    break;
                }

                var name = Regex.Match(lines[i], @"(?<=\s*).*(?=\s\d)").Value.Trim();
                var value = long.Parse(Regex.Match(lines[i], @"(?<=.*\s)\d+").Value.Replace(",", string.Empty));
                items.Add(new CountField
                {
                    Item = name,
                    Count = value
                });
            }

            i--;
            return items;
        }

        private static (int, string, string) GetTechRarityAndType(string[] lines)
        {
            var startIdx = Array.IndexOf(lines, lines.First(l => l.StartsWith("Most profitable ")));
            for (var i = startIdx + 1; i < lines.Length; ++i)
            {
                if (lines[i].Length <= 0 || lines[i][0] == ' ')
                {
                    continue;
                }

                var tech = 0;
                if (lines[i].StartsWith("Tech "))
                {
                    tech = int.Parse(Regex.Match(lines[i], @"(?<=Tech )\d*").Value);
                    lines[i] = Regex.Replace(lines[i], @"Tech \d*\s", string.Empty);
                }

                var rarityPivot = lines[i].IndexOf(' ');
                return (
                    tech,
                    lines[i].Substring(0, rarityPivot),
                    lines[i].Substring(rarityPivot + 1, lines[i].Length - rarityPivot - 1)
                );
            }

            throw new InvalidDataException();
        }

        private static long GetAverageBuyPrice(IEnumerable<string> lines)
        {
            var sellLine = lines.First(l => l.StartsWith("   Average buying price (from shop):"));
            var price = sellLine.Split(':')[1];
            return long.Parse(price.Replace(",", string.Empty));
        }

        private static long GetAverageSellPrice(IEnumerable<string> lines)
        {
            var sellLine = lines.First(l => l.StartsWith("   Average selling price (to shop):"));
            var price = sellLine.Split(':')[1];
            return long.Parse(price.Replace(",", string.Empty));
        }

        private static string GetName(IReadOnlyList<string> messageLines)
        {
            return messageLines[0];
        }

        private class CountField
        {
            public string Item { get; set; }

            public long Count { get; set; }
        }

        private class AugStat
        {
            public string Stat { get; set; }

            public double Amount { get; set; }

            public bool IsFlat { get; set; }
        }
    }
}