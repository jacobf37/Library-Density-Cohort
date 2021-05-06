//  Authors:  Robert M. Scheller, James B. Domingo

namespace Landis.Library.DensityCohorts
{
    /// <summary>
    /// A species cohort with number of tree information.
    /// </summary>
    public interface ICohort
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

       // ushort Age
        //{
        //    get;
        //}
        //---------------------------------------------------------------------

        /// <summary>
        /// Computes the relative density of a cohort.
        /// </summary>
        /// <param name="cohort">
        /// The site where the cohort is located.
        /// </param>
        float ComputeCohortRD(Cohort cohort);

        void ChangeTreenumber(int delta);

        void IncrementAge();
    }
}
