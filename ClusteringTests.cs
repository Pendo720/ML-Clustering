using KmsLibraryCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Clustering
{
    public class PipelineTests
    {
        [Fact]
        public void Default_Data_Split_Ratio()
        {
            // arrange 
            var Data = Enumerable.Range(1, 10).ToList();
            MLDataPipeline<int> pipeline = new MLDataPipeline<int>(Data);

            // act
            var train = pipeline.Training;
            var test = pipeline.Testing;
            var crossValidate = pipeline.CrossValidation;

            // assert
            Assert.Equal(train.Count + test.Count + crossValidate.Count, Data.Count);
            Assert.Equal(train.Count, 0.6f * Data.Count);
        }

        [Fact]
        public void Verify_Data_Split_Ratio()
        {
            // arrange 
            var Data = Enumerable.Range(1, 10).ToList();
            MLDataPipeline<int> pipeline = new MLDataPipeline<int>(Data, 0.8f);

            // act
            var train = pipeline.Training;
            var test = pipeline.Testing;
            var crossValidate = pipeline.CrossValidation;

            // assert
            Assert.Equal(train.Count + test.Count + crossValidate.Count, Data.Count);
            Assert.Equal(train.Count, 0.8 * Data.Count);
        }

        [Fact]
        public void Verify_CsvData_Import()
        {
            // arrange 
            //var Data = MLDataPipeline<Feature<Field>>.ImportCSV(@"C:\Susu-ilo\Projects\Tracked-Git\KmeansTests\Data\AppData.csv");
            // act

            // assert
            //Assert.NotNull(Data);
            //Assert.Equal(3, Data.Count);
        }
    }

    public class KmsAlgorithmTests
    {
        [Fact]
        public void Creation()
        {
            // arrange 
            var features = new List<Feature<Field>>();
            Enumerable.Range(0, 64).ToList().ForEach(i =>
            {
                var feature = new Feature<Field>();
                "xyz".ToList().ForEach(c => feature.Fields.Add(new Field(Char.ToString(c), (float)new Random().NextDouble())));
                features.Add(feature);
            });

            MLDataPipeline<Feature<Field>> pipeline = new MLDataPipeline<Feature<Field>>(features);
            var kms = new KmsAlgorithm<Field>(pipeline.Training, new List<String>() { "Apples", "Mangoes", "Bananas" });
            // act
            kms.runIterations();

            // assert
            Assert.True(kms.Clusters.Count == 3);
        }
    }

    public class ClusterTests
    {
        [Fact]
        public void Creation()
        {
            // arrange 
            var fcluster = new Cluster<Feature<Field>>();

            var features = new List<Feature<Field>>();
            Enumerable.Range(1, 10).ToList().ForEach(i =>
            {
                var feature = new Feature<Field>();
                new String[] { "x", "y", "z", "signal" }
                .ToList()
                .ForEach(c => feature.Fields.Add(new Field(c, (float)new Random().NextDouble())));
                features.Add(feature);
            });

            var centre = new Feature<Field>();
            new String[] { "x", "y", "z", "signal" }
                        .ToList()
                        .ForEach(c => centre.Fields.Add(new Field(c, (float)new Random().NextDouble())));
            // act
            fcluster.Centroid.Fields.Add(centre);

            // assert
            Assert.NotNull(fcluster);
            Assert.Equal(fcluster.Centroid.Fields.ElementAt(0), centre);
            Assert.True(1 == fcluster.Centroid.Fields.Count);
        }
    }

    public class FeatureTests
    {
        [Fact]
        public void Creation_()
        {
            // arrange 
            var features = new List<Field>(){
                new Field("z", (float)new Random().NextDouble()),
                new Field("y", (float)new Random().NextDouble()),
                new Field("x", (float)new Random().NextDouble())
            };
            Feature<Field> ff = new Feature<Field>(features);

            // act
            String asText = ff.ToString();

            // assert
            Assert.NotNull(ff);
            Assert.True(ff.Fields.Count == 3);
            Assert.True(ff.ToString().Contains('x'));
        }
    }

    public class FieldTests
    {
        [Fact]
        public void Creation()
        {
            // arrange
            Field ff = new Field("x", 21.54f);

            // act
            String asText = ff.ToString();

            // assert
            Assert.NotNull(ff);
            Assert.True(asText.Contains('x'));
            Assert.True(ff.Value == 21.54f);
        }

        [Fact]
        public void Normalisation()
        {
            // arrange
            float min = 3.51f, max = 24.0f, cvalue = 21.54f;
            Field ff = new Field("x", cvalue);

            // act
            float result = ff.Normalise(min, max);

            // assert
            Assert.NotNull(ff);
            Assert.True((float)(cvalue - min) / max == result);
        }
    }
}
