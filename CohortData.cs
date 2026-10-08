using Landis.Core;
using Landis.Library.UniversalCohorts;

namespace Landis.Library.DensityCohorts
{
    /// <summary>
    /// Data for an individual cohort that is not shared with other cohorts.
    /// </summary>
    public struct CohortData
    {
        /// <summary>
        /// The cohort
        /// </summary>
        public Cohort Cohort;

        /// <summary>
        /// The universal cohort data
        /// </summary>
        public UniversalCohorts.CohortData UniversalData;

        /// <summary>
        /// The cohort's age (years).
        /// </summary>
        public ushort Age;

        //---------------------------------------------------------------------

        /// <summary>
        /// The cohort's biomass (g/m2).
        /// </summary>
        public int Biomass;

        //---------------------------------------------------------------------
        /// <summary>
        /// The cohort's diameter (cm).
        /// </summary>
        public float Diameter;
        
        //---------------------------------------------------------------------

        /// <summary>
        /// The number of individual trees in the cohort.
        /// </summary>
        public int Treenumber;

        //---------------------------------------------------------------------


        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="age">
        /// The cohort's age.
        /// </param>
        /// <param name="treenumber">
        /// The number of trees in the cohort.
        /// </param>
        public CohortData(ushort age,
                          int treenumber,
                          int biomass,
                          int diameter,
                          double ANPP
                          )
        {
            this.Age = age;
            this.Treenumber = treenumber;
            this.Biomass = 0;
            this.Diameter = 0;
            this.UniversalData.Age = age;
            this.UniversalData.ANPP = ANPP;
            this.UniversalData.Biomass = biomass;
        }

        public CohortData(Cohort cohort)
        {
            this.Age = cohort.Age;
            this.Treenumber = cohort.Treenumber;
            this.Biomass = cohort.Biomass;
            this.Diameter = cohort.Diameter;
            this.UniversalData.Age = cohort.Age;
            this.UniversalData.Biomass = cohort.Biomass;
            this.UniversalData.ANPP = cohort.Data.UniversalData.ANPP;
        }
    }
}
