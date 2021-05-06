//  Authors:  Robert M. Scheller, James B. Domingo

using Landis.Core;
using System.Collections.Generic;
using System.IO;

namespace Landis.Library.DensityCohorts
{

    public class DynamicInputs
    {
        private static Dictionary<int, IDynamicInputRecord[,]> allData;
        private static IDynamicInputRecord[,] timestepData;

        public DynamicInputs()
        {
        }

        public static Dictionary<int, IDynamicInputRecord[,]> AllData
        {
            get {
                return allData;
            }
        }
        //---------------------------------------------------------------------
        public static IDynamicInputRecord[,] TimestepData
        {
            get {
                return timestepData;
            }
            set {
                timestepData = value;
            }
        }

        public static void Write()
        {
            foreach(ISpecies species in EcoregionData.ModelCore.Species)
            {
                foreach(IEcoregion ecoregion in EcoregionData.ModelCore.Ecoregions)
                {
                    if (!ecoregion.Active)
                        continue;

                    EcoregionData.ModelCore.UI.WriteLine("Spp={0}, Eco={1}, Pest={2:0.0}.", species.Name, ecoregion.Name,
                        timestepData[species.Index, ecoregion.Index].ProbEst);

                }
            }

        }
        //---------------------------------------------------------------------
        public static void Initialize(string filename, bool writeOutput)
        {
            EcoregionData.ModelCore.UI.WriteLine("   Loading dynamic input data from file \"{0}\" ...", filename);
            DynamicInputsParser parser = new DynamicInputsParser();
            try
            {
                allData = Landis.Data.Load<Dictionary<int, IDynamicInputRecord[,]>>(filename, parser);
            }
            catch (FileNotFoundException)
            {
                string mesg = string.Format("Error: The file {0} does not exist", filename);
                throw new System.ApplicationException(mesg);
            }

            timestepData = allData[0];
        }
    }

}
