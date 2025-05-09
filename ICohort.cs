//  Authors:  Robert M. Scheller, James B. Domingo

using Landis.Core;
using Landis.SpatialModeling;

namespace Landis.Library.DensityCohorts
{
    /// <summary>
    /// A species cohort with number of tree information.
    /// </summary>
    public interface ICohort
    //    : Landis.Library.AgeOnlyCohorts.ICohort, BiomassCohorts.ICohort, Landis.Library.Cohorts.ICohort
    :BiomassCohorts.ICohort, Landis.Library.AgeOnlyCohorts.ICohort
    {
        /// <summary>
        /// The number of individual trees in the cohort.
        /// </summary>
        int Treenumber
        {
            get;
        }

        float Diameter
        {
            get;
        }

        int Biomass
        {
            get;
        }

        CohortData Data
        {
            get;
        }

        ushort Age
        {
            get;
        }

        ISpecies Species 
        { 
            get; 
        }

        //---------------------------------------------------------------------

        /// <summary>
        /// Computes the relative density of a cohort.
        /// </summary>
        /// <param name="cohort">
        /// The site where the cohort is located.
        /// </param>
        float ComputeCohortRD(Cohort cohort);

        double ComputeCohortBasalArea(ICohort cohort);

        int ComputeCohortBiomass(ICohort cohort);

        int ComputeNonWoodyBiomass(ActiveSite site);

        void ChangeTreenumber(int delta);

        void IncrementAge();
    }
}
