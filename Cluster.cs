using System;
using System.Collections.Generic;
using System.Linq;

namespace KmsLibraryCode
{
    public sealed class Cluster<T>
    {
        public String Label { get; set; }

        public Feature<T> Centroid { get; set; }
        public List<Feature<T>> Elements { get; set; }

        public float Radius { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="centroid"></param>
        /// <param name="label"></param>
        public Cluster(Feature<T> centroid, String label)
        {
            Centroid = centroid;
            Label = label;
            Elements = new List<Feature<T>>();
            Radius = 0f;
        }
        /// <summary>
        /// 
        /// </summary>
        public Cluster()
        {
            Centroid = new Feature<T>();
            Label = "";
            Elements = new List<Feature<T>>();
            Radius = 0f;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string toReturn = String.Format("Label: {0}\nCentroid: {1} Elements({2})\n", Label, Centroid, Elements.Count);
            Elements.Select(m => toReturn += m.ToString()).ToList();
            return toReturn;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Feature<T> AdjustCentroid()
        {
            var target = new Feature<Field>();
            var fields = new List<Field>();
            var all = new List<Field>();
            Elements.ForEach(f => all.AddRange(f.Fields as List<Field>));
            if (all.Count != 0)
            {
                Centroid.Reset();
                target.Fields.Clear();
                fields.Clear();
                all.GroupBy(p => p.Name)
                    .ToList()
                    .Select(p => new { Name = p.Key, Value = p.Average(s => s.Value) })
                    .ToList()
                    .ForEach(p =>
                    {
                        var toAdd = new Field(p.Name, p.Value);
                        target.Fields.Add(toAdd);
                    });
            }

            return target as Feature<T>;
        }
        /// <summary>
        /// 
        /// </summary>
        public void UpdateFieldRanges()
        {
            if (Elements.Count > 0)
            {
                Radius = Centroid.distanceTo(Elements.Aggregate((p, q) => Centroid.distanceTo(p) > Centroid.distanceTo(q) ? p : q));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        public Feature<T> IsWithin(Feature<T> feature)
        {
            if (Radius != 0f)
            {
                var where = Centroid.distanceTo(feature);
                return where < Radius ? Centroid : null;
            }

            return null;
        }
    }
}
