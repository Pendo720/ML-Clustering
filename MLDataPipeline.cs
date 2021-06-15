using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace KmsLibraryCode
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MLDataPipeline<T>
    {
        public ImmutableList<T> Data { get; }
        public List<T> Training { get; set; }
        public List<T> Testing { get; set; }
        public List<T> CrossValidation { get; set; }

        /// <summary>
        ///     Constructs a pipeline for holding and splitting model data 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="trainRatio"></param>
        public MLDataPipeline(List<T> data, float trainRatio = 0.6f)
        {
            Data = data.ToImmutableList();
            Split(trainRatio);
        }
        /// <summary>
        ///     Splits the data into training, testing and cross validation sets
        ///     The default splitting ratio is 60%, 20% and 20%
        /// </summary>
        /// <param name="training"></param>
        private void Split(float training = 0.6f)
        {
            int train = (int)(training * Data.Count);
            int other = (int)(0.5 * (1 - training) * Data.Count);
            Training = Data.Take(train).ToList();
            Testing = Data.Skip(train).Take(other).ToList();
            CrossValidation = Data.Skip(train + other).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <param name="times"></param>
        /// <returns></returns>
        /*public static List<T> ShuffleAndDuplicate(List<T> items, int times)
        {
            items.OrderBy(f => new Random().Next());
            Enumerable.Range(0, times / 2).ToList().ForEach(p =>
              {
                  items.AddRange(items);
              });
            return items;
        }*/

        /// <summary>
        ///     Normalise feature in readiness for processing
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static List<Feature<Field>> Normalise(List<Feature<Field>> items)
        {
            if (items != null && items.Count > 1)
            {
                items.ForEach(x => Console.WriteLine(x.ToString().Trim()));
                var all = new List<Field>();
                items.ForEach(i => all.AddRange(i.Fields));

                var minmax = all.GroupBy(p => (p as Field).Name)
                                .ToList()
                                .Select(p => new { Name = p.Key, Min = p.Min(s => (s as Field).Value), Max = p.Max(s => (s as Field).Value) })
                                .ToList();
                items.ForEach(f =>
                {
                    for (int i = 0; i < f.Fields.Count; i++)
                    {
                        var limits = minmax.ElementAt(i);
                        var field = f.Fields.ElementAt(i);
                        if (field.Name == limits.Name)
                        {
                            field.Value = field.Normalise(limits.Min, limits.Max);
                        }
                    }
                });

                Console.WriteLine("Normalised features");
                items.ForEach(x => Console.WriteLine(x.ToString().TrimEnd()));
            }

            return items;
        }
        /// <summary>
        ///     Imports input data from a csv file
        /// </summary>
        /// <param name="csvfile"></param>
        /// <returns>
        ///     List of features contained in the the file with each line 
        ///     corresponding to an item
        /// </returns>
        public static List<Feature<Field>> ImportCSV(string csvfile)
        {
            var items = new List<Feature<Field>>();
            try
            {
                if (File.Exists(csvfile))
                {

                    using (StreamReader sr = File.OpenText(csvfile))
                    {
                        string s = string.Empty;
                        string[] sLabels = sr.ReadLine().Split(',');
                        while ((s = sr.ReadLine()) != null)
                        {
                            Feature<Field> feature = new Feature<Field>();
                            string[] values = s.Split(',');
                            for (int i = 0; i < sLabels.Length; i++)
                            {
                                //float fNorm = (float)(1f / (1 + Math.Exp(-float.Parse(values[i]))));
                                float fNorm = float.Parse(values[i]);
                                feature.Add(new Field(sLabels[i], fNorm));
                            }

                            items.Add(feature);
                        }
                    }
                }
                else
                {
                    Console.WriteLine(csvfile + " does not exists");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return Normalise(items);
        }
    }
}
