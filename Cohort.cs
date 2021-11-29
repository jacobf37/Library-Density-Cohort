//  Authors:  Robert M. Scheller, James B. Domingo

using Landis.Core;
using Landis.SpatialModeling;
using Landis.Utilities;
using System;
using System.Collections.Generic;


namespace Landis.Library.DensityCohorts
{
    /// <summary>
    /// A species cohort with density information.
    /// </summary>
    public class Cohort
        : Landis.Library.DensityCohorts.ICohort
        //Landis.Library.DensityCohorts.ICohort, Landis.Library.AgeOnlyCohorts.ICohort, Landis.Library.BiomassCohorts.ICohort
    {

        private ISpecies species;
        private ISpeciesDensity speciesDensity;
        private CohortData data;
        private ushort age;
        private int treenumber;
        private bool firstYear;
        private float diameter;
        public static IEcoregion ecoregion;
        public static ActiveSite site;
        
        //---------------------------------------------------------------------

        public ISpecies Species
        {
            get {
                return species;
            }
        }
        //---------------------------------------------------------------------

        public ISpeciesDensity DensitySpecies
        {
            get
            {
                return speciesDensity;
            }
        }
        //---------------------------------------------------------------------

        public ushort Age
        {
            get {
                return data.Age;
            }
        }

        //---------------------------------------------------------------------

        public int Biomass
        {
            get
            {
                //JSF - Output for Biomass in FIA units
                
                ISpeciesDensity speciesdensity = SpeciesParameters.SpeciesDensity.AllSpecies[species.Index];
                float bioCoef_1 = SpeciesParameters.biomass_util.GetBiomassData(speciesdensity.BiomassClass, 1);
                float bioCoef_2 = SpeciesParameters.biomass_util.GetBiomassData(speciesdensity.BiomassClass, 2);
                double biomass = Math.Exp(bioCoef_1 + bioCoef_2 * Math.Log(this.data.Diameter)) * 2.2046 * this.data.Treenumber;
                return (int)biomass;

            }
        }

        //---------------------------------------------------------------------

        public float Diameter
        {
            get
            {
                //FIXME - this seems to be working
                float diameter = 0;
                
                Dictionary<int, double> diameters = DiameterInputs.AllData[ecoregion.Name][species.Name].Diameters;
                if (diameters.ContainsKey(Age))
                {
                    diameter = (float)diameters[Age];
                }
                else
                {
                    for (int i = Age; i > 0; i--)
                    {
                        if (diameters.ContainsKey(i))
                        {
                            diameter = (float)diameters[i];
                        }
                    }
                }
                return diameter;
            }
        }

        //---------------------------------------------------------------------

        //---------------------------------------------------------------------

        public float cohortDiameter(ActiveSite site)
        {
        
                //FIXME - this seems to be working
            float diameter = 0;
            
            Dictionary<int, double> diameters = DiameterInputs.AllData[ecoregion.Name][species.Name].Diameters;
            if (diameters.ContainsKey(Age))
            {
                diameter = (float)diameters[Age];
            }
            else
            {
                for (int i = Age; i > 0; i--)
                {
                    if (diameters.ContainsKey(i))
                    {
                        diameter = (float)diameters[i];
                    }
                }
            }
            return diameter;
            
        }

        //---------------------------------------------------------------------

        public void Accumulate(Cohort c)
        {
            data.Treenumber += c.data.Treenumber;
        }

        //---------------------------------------------------------------------

        public int Treenumber
        {
            get
            {
                return data.Treenumber;
            }
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// The cohort's age and density data.
        /// </summary>
        public CohortData Data
        {
            get {
                return data;
            }
        }


        //---------------------------------------------------------------------
        // Constructor
        public Cohort(ISpecies species, ushort year_of_birth, string SiteName, int treenumber)
        {
            this.species = species;
            this.speciesDensity = SpeciesParameters.SpeciesDensity.AllSpecies[species.Index];
            this.data.Age = 1;

            //FIXME JSF- initialize the number of trees in a new cohort
            this.data.Treenumber = treenumber;

            firstYear = true;
        }

        //---------------------------------------------------------------------

        public static void SetSiteAccessFunctions(SiteCohorts sitecohorts)
        {
            Cohort.ecoregion = sitecohorts.Ecoregion;
            Cohort.site = sitecohorts.Site;
        }

        //---------------------------------------------------------------------

        public static void RaiseDeathEvent(object sender,
                    Cohort cohort,
                    ActiveSite site,
                    ExtensionType disturbanceType)
        {
            if (DeathEvent != null)
            {
                DeathEvent(sender, new Landis.Library.DensityCohorts.DeathEventArgs(cohort, site, disturbanceType));
            }

        }

        //---------------------------------------------------------------------

        public Cohort(ISpecies species,
                      ushort   age,
                      int   treenumber,
                      IEcoregion ecoregion)
        {
            this.species = (ISpecies) species;
            this.speciesDensity = SpeciesParameters.SpeciesDensity.AllSpecies[species.Index];
            this.data.Age = age;
            this.data.Treenumber = treenumber;
            this.data.Diameter = 0;
            this.data.Biomass = 0;

            if (ecoregion.Active)
            {
                float diameter = 0;
                Dictionary<int, double> diameters = DiameterInputs.AllData[ecoregion.Name][species.Name].Diameters;
                if (diameters.ContainsKey(Age))
                {
                    diameter = (float)diameters[Age];
                }
                else
                {
                    for (int i = Age; i > 0; i--)
                    {
                        if (diameters.ContainsKey(i))
                        {
                            diameter = (float)diameters[i];
                        }
                    }
                }
                this.data.Diameter = diameter;
                //FIXME ---- JSF
                //ISpeciesDensity speciesdensity = SpeciesParameters.SpeciesDensity.AllSpecies[species.Index];
                double biomass = Math.Exp(SpeciesParameters.biomass_util.GetBiomassData(this.speciesDensity.BiomassClass, 1) + SpeciesParameters.biomass_util.GetBiomassData(this.speciesDensity.BiomassClass, 2) * Math.Log(diameter)) * data.Treenumber / 1000.00; // Mg/cell
                int biomass_int = System.Convert.ToInt32(biomass);
                double biomass_gm2 = biomass * 1000 * 1000 / (EcoregionData.ModelCore.CellLength * EcoregionData.ModelCore.CellLength);
                int biomass_gm2_int = System.Convert.ToInt32(biomass_gm2);
                this.data.Biomass = biomass_gm2_int;
            }
        }

        //---------------------------------------------------------------------

        public Cohort(ISpecies   species,
                      CohortData cohortData)
        {
            this.species = (ISpecies) species;
            this.speciesDensity = SpeciesParameters.SpeciesDensity.AllSpecies[species.Index];
            this.data = cohortData;
        }

        //---------------------------------------------------------------------

        //---------------------------------------------------------------------

        public Cohort(ISpecies species,
                      ushort age,
                      int treenumber)
        {
            this.species = (ISpecies)species;
            this.speciesDensity = SpeciesParameters.SpeciesDensity.AllSpecies[species.Index];
            this.data.Age = age;
            this.data.Treenumber = treenumber;
            this.data.Diameter = 0;
            this.data.Biomass = 0;
        }

        //---------------------------------------------------------------------

        public Cohort(Cohort cohort) // : base(cohort.species, new Landis.Library.BiomassCohorts.CohortData(cohort.age, cohort.Biomass))
        {
            this.species = (ISpecies) cohort.Species;
            this.speciesDensity = SpeciesParameters.SpeciesDensity.AllSpecies[this.species.Index];
            this.data.Age = cohort.Age;
            this.data.Treenumber = cohort.Treenumber;
            this.diameter = cohort.Diameter;
            this.data.Biomass = cohort.Biomass;
        }

        //---------------------------------------------------------------------

        public Cohort(ISpecies species, ushort age, int treenumber, string SiteName, ushort firstYear, IEcoregion siteEcoregion)
        {
            //InitializeSubLayers();
            this.species = (ISpecies) species;
            this.speciesDensity = SpeciesParameters.SpeciesDensity.AllSpecies[species.Index];
            ecoregion = siteEcoregion;
            this.data.Age = age;
            this.data.Treenumber = treenumber;
            if (ecoregion.Active)
            {
                float diameter = 0;
                Dictionary<int, double> diameters = DiameterInputs.AllData[ecoregion.Name][species.Name].Diameters;
                if (diameters.ContainsKey(Age))
                {
                    diameter = (float)diameters[Age];
                }
                else
                {
                    for (int i = Age; i > 0; i--)
                    {
                        if (diameters.ContainsKey(i))
                        {
                            diameter = (float)diameters[i];
                        }
                    }
                }
                this.data.Diameter = diameter;
                //FIXME ---- JSF
                //ISpeciesDensity speciesdensity = SpeciesParameters.SpeciesDensity.AllSpecies[species.Index];
                float bioCoef_1 = SpeciesParameters.biomass_util.GetBiomassData(this.speciesDensity.BiomassClass, 1);
                float bioCoef_2 = SpeciesParameters.biomass_util.GetBiomassData(this.speciesDensity.BiomassClass, 2);
                double biomass = Math.Exp(bioCoef_1 + bioCoef_2 * Math.Log(diameter)) * 2.2046 * this.data.Treenumber; // Mg/cell
                //int biomass_int = System.Convert.ToInt32(biomass);
                //double biomass_gm2 = biomass * 1000 * 1000 / (EcoregionData.ModelCore.CellLength * EcoregionData.ModelCore.CellLength);
                //int biomass_gm2_int = System.Convert.ToInt32(biomass_gm2);
                //this.data.Biomass = biomass_gm2_int;
                this.data.Biomass = (int)biomass;
            }

        }

        //---------------------------------------------------------------------

        /// <summary>
        /// Increments the cohort's age by one year.
        /// </summary>
        public void IncrementAge()
        {
            int successionTimestep = Landis.Library.DensityCohorts.Cohorts.SuccessionTimeStep;

            if (data.Age % successionTimestep == 0)
            {
                ushort newAge = (ushort)(data.Age + successionTimestep);
                data.Age = newAge;
            } 
            else
            {
                ushort newAge = (ushort)(((age / successionTimestep) + 1) * successionTimestep);
                data.Age = newAge;
            }
                
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// Changes the cohort's tree number.
        /// </summary>
        public void ChangeTreenumber(int delta)
        {
            int newTreenumber = data.Treenumber + delta;
            data.Treenumber = System.Math.Max(0, newTreenumber);
            //if (data.Treenumber == 0)
            //{
            //    RemoveCohort(cohort, null);
            //}
            data.Biomass = this.Biomass;
        }

        //---------------------------------------------------------------------

        public double ComputeCohortBasalArea(ICohort cohort)
        {
            double local_const = 3.1415926 / (4 * 10000.00);

            double cohortBA = Math.Pow(cohort.Diameter, 2) * local_const * cohort.Treenumber;

            return cohortBA;
        }

        //---------------------------------------------------------------------

        public int ComputeNonWoodyBiomass(ActiveSite site)
        {
            Percentage nonWoodyPercentage = Cohorts.BiomassCalculator.ComputeNonWoodyPercentage(this, site);
            return (int)(data.Biomass * nonWoodyPercentage);
        }

        //---------------------------------------------------------------------

        public float ComputeCohortRD(Cohort cohort)
        {
            ISpeciesDensity speciesDensity = SpeciesParameters.SpeciesDensity.AllSpecies[cohort.Species.Index];

            float tmp_term1 = (float)Math.Pow((cohort.Diameter / 25.4), 1.605);
            float tmp_term2 = 10000 / speciesDensity.MaxSDI;
            int tmp_term3 = cohort.Treenumber;
            float cohortRD = tmp_term1 * tmp_term2 * tmp_term3 / (float)Math.Pow(EcoregionData.ModelCore.CellLength, 2);
            return cohortRD;
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// Occurs when a cohort dies either due to senescence or biomass
        /// disturbances.
        /// </summary>
        public static event DeathEventHandler<DeathEventArgs> DeathEvent;
        //---------------------------------------------------------------------
        public static event PartialDeathEventHandler<PartialDeathEventArgs> PartialDeathEvent;

        /// <summary>
        /// Raises a Cohort.DeathEvent if partial mortality.
        /// </summary>
        public static void PartialMortality(object sender,
                                ICohort cohort,
                                ActiveSite site,
                                ExtensionType disturbanceType,
                                float reduction)
        {
            if (PartialDeathEvent != null)
                PartialDeathEvent(sender, new PartialDeathEventArgs(cohort, site, disturbanceType, reduction));
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// Raises a Cohort.DeathEvent.
        /// </summary>
        public static void Died(object     sender,
                                ICohort    cohort,
                                ActiveSite site,
                                ExtensionType disturbanceType)
        {
            if (DeathEvent != null)
                DeathEvent(sender, new DeathEventArgs(cohort, site, disturbanceType));
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// Occurs when a cohort is killed by an age-only disturbance.
        /// </summary>
        public static event DeathEventHandler<DeathEventArgs> AgeOnlyDeathEvent;

        //---------------------------------------------------------------------

        /// <summary>
        /// Raises a Cohort.AgeOnlyDeathEvent.
        /// </summary>
        public static void KilledByAgeOnlyDisturbance(object     sender,
                                                      ICohort    cohort,
                                                      ActiveSite site,
                                                      ExtensionType disturbanceType)
        {
            if (AgeOnlyDeathEvent != null)
                AgeOnlyDeathEvent(sender, new DeathEventArgs(cohort, site, disturbanceType));
        }
    }
}
