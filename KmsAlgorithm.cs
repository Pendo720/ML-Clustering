using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KmsLibraryCode
{
    /// <summary>
    ///     The public api for running k-means clustering algorithm
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class KmsAlgorithm<T>
    {
        static int Counter = 0;
        public ImmutableList<Feature<T>> Features { get; set; }
        public List<Cluster<T>> Clusters { get; set; }
        public List<Feature<T>> Centroids { get; set; }
        public int NumOfClusters { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="features"></param>
        /// <param name="labels"></param>
        public KmsAlgorithm(List<Feature<T>> features, List<String> labels)
        {
            Clusters = new List<Cluster<T>>();
            Features = features.ToImmutableList();
            NumOfClusters = labels.Count;
            initCentroids(labels);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="features"></param>
        /// <param name="centroids"></param>
        public KmsAlgorithm(List<Feature<T>> features, List<Feature<T>> centroids)
        {
            Clusters = new List<Cluster<T>>();
            Features = features.ToImmutableList();
            Centroids = centroids;
            NumOfClusters = Centroids.Count;
            Centroids.ToList().ForEach(c => Clusters.Add(new Cluster<T>(c, "A")));
        }

        public KmsAlgorithm()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="labels"></param>
        private void initCentroids(List<String> labels)
        {
            Console.WriteLine("Randomly initialised centroids...");
            Centroids = new List<Feature<T>>();
            var rndIndices = Enumerable.Range(0, labels.Count).Select(x => new Random().Next(0, Features.Count)).ToList();

            rndIndices.ForEach(i =>
            {
                if (Features.Count > 0)
                {
                    Centroids.Add(Features.ElementAt(i));
                }
            });

            foreach (var item in Centroids.Select((value, i) => (value, i)))
            {
                Clusters.Add(new Cluster<T>(Centroids.ElementAt(item.i), labels.ElementAt(item.i)));
            }

            initialAssignment();
        }
        /// <summary>
        /// 
        /// </summary>
        private void initialAssignment()
        {
            Features.ForEach(f => Clusters
                                    .Aggregate((p, q) => f.distanceTo(p.Centroid) < f.distanceTo(q.Centroid) ? p : q)
                                    .Elements.Add(f));

            Clusters.ForEach(r => r.UpdateFieldRanges());
        }
        /// <summary>
        /// 
        /// </summary>
        public void runIterations()
        {
            Counter += 1;
            Console.WriteLine("Running iteration: " + Counter);
            Boolean repeat = true;

            List<Feature<T>> newCentroids = new List<Feature<T>>();
            var clusters = Clusters;
            Clusters.ForEach(c =>
            {
                Console.WriteLine(c.ToString());
                Feature<T> nC = c.AdjustCentroid();
                c.Centroid = nC;
                newCentroids.Add(nC);
                Console.WriteLine("New Centroid: " + nC.ToString());
            });

            if (newCentroids.Count > 0)
            {
                Enumerable.Range(0, Clusters.Count)
                            .ToList()
                            .ForEach(i =>
                            {
                                var cc = clusters.ElementAt(i).Centroid;
                                var nc = newCentroids.ElementAt(i);
                                for (int j = 0; j < cc.Fields.Count; j++)
                                {
                                    repeat &= cc.Fields.ElementAt(j).Equals(nc.Fields.ElementAt(j));
                                }
                            });

                if (!repeat)
                {
                    Centroids = newCentroids;

                    Enumerable.Range(0, newCentroids.Count)
                            .ToList()
                            .ForEach(c =>
                            {
                                Clusters.ElementAt(c).Elements.Clear();
                                Clusters.ElementAt(c).Centroid = newCentroids.ElementAt(c);
                            });

                    Features.ForEach(f => Clusters
                                        .Aggregate((p, q) => p.Centroid.distanceTo(f) < q.Centroid.distanceTo(f) ? p : q)
                                        .Elements.Add(f));
                    Clusters.ForEach(c => c.UpdateFieldRanges());
                    runIterations();
                }
            }

            /*            ExportClusters("csvexport");
                        LoadClusters("csvexport.csv");*/
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string toReturn = string.Empty;
            Centroids.ToList().ForEach(c => toReturn += c.ToString());
            return toReturn;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        public Cluster<T> Classify(Feature<T> feature)
        {
            Cluster<T> toReturn = null;
            for (int i = 0; i < Clusters.Count; i++)
            {
                toReturn = Clusters.ElementAt(i);
                if (toReturn.IsWithin(feature) != null)
                {
                    return toReturn;
                }
            }

            return toReturn;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="csvfile"></param>
        public async Task ExportClusters(string csvfile)
        {
            // Create the file, or overwrite if the file exists.
            await using (FileStream fs = File.Create(csvfile))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(JsonSerializer.Serialize(Clusters).ToString());
                fs.Write(info, 0, info.Length);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="csvfile"></param>
        public Task ImportClusters(string csvfile)
        {
            using StreamReader sr = File.OpenText(csvfile);
            string sText = sr.ReadToEndAsync().Result;
            Clusters = JsonSerializer.Deserialize<List<Cluster<T>>>(sText);
            return Task.CompletedTask;
        }
    }
}
