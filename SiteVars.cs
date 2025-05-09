using System;
using System.Collections.Generic;
using System.Text;

using Landis.Core;
using Landis.Library.DensityCohorts;
using Landis.Library.AgeOnlyCohorts;
using Landis.SpatialModeling;
using Landis.Library.Metadata;
using System.Reflection;
using Landis.Library.SnagCohorts;
using System.Linq;


namespace Landis.Library.DensityCohorts
{

    public static class SiteVars
    {
        private static ISiteVar<float> siteRD;
        private static ISiteVar<Landis.Library.DensityCohorts.SiteCohorts> sitecohorts;

        public static MetadataTable<SummaryLogMortality> summaryLogMortality;

        public static ISiteVar<double> fineFuels;
        public static ISiteVar<ISiteSnagCohorts> SnagCohorts { get; private set; }

        public static void Initialize()
        {
            siteRD = EcoregionData.ModelCore.Landscape.NewSiteVar<float>();
            fineFuels = EcoregionData.ModelCore.Landscape.NewSiteVar<double>();

            EcoregionData.ModelCore.RegisterSiteVar(siteRD, "Succession.SiteRd");
            EcoregionData.ModelCore.RegisterSiteVar(fineFuels, "Succession.FineFuels");

            
        }

        public static void InitializeSnags()
        {
            SnagCohorts = EcoregionData.ModelCore.GetSiteVar<ISiteSnagCohorts>("Succession.SnagCohorts");
        }
        public static void SpeciesSiteRD(Landis.Library.DensityCohorts.SpeciesCohorts speciesCohorts, ActiveSite site)
        {
            float siteRD = 0;

            ISpeciesDensity speciesDensity = SpeciesParameters.SpeciesDensity.AllSpecies[speciesCohorts.Species.Index];
            //foreach (Landis.Library.DensityCohorts.ICohort cohort in speciesCohorts)
            for (int s = 0; s < speciesCohorts.Count; s++)
            {
                Landis.Library.DensityCohorts.ICohort cohort = speciesCohorts[s];
                double tmp_term1 = Math.Pow((cohort.Diameter / 25.4), 1.605);
                float tmp_term2 = 10000 / speciesDensity.MaxSDI;
                int tmp_term3 = cohort.Treenumber;
                double tmp = tmp_term1 * tmp_term2 * tmp_term3 / Math.Pow(EcoregionData.ModelCore.CellLength, 2);
                siteRD += (float)tmp;
            }

            SiteVars.SiteRD[site] = siteRD;
            //return siteRD;
        }

        public static void TotalSiteRD(Landis.Library.DensityCohorts.SiteCohorts cohorts)
        {

            float siteRD = 0;
            Landis.Library.DensityCohorts.Cohort.SetSiteAccessFunctions(cohorts);
            if (cohorts == null)
            {
                SiteVars.SiteRD[cohorts.Site] = siteRD;
            }
            else
            {
                foreach (Landis.Library.DensityCohorts.ICohort cohort in cohorts.AllCohorts)
                {
                    if (SpeciesParameters.SpeciesDensity.AllSpecies[cohort.Species.Index].SpType == 3)
                    {
                        siteRD += 0;
                    }
                    else
                    {
                        double tmp_term1 = Math.Pow((cohort.Diameter / 25.4), 1.605);
                        float tmp_term2 = 10000 / SpeciesParameters.SpeciesDensity.AllSpecies[cohort.Species.Index].MaxSDI;
                        int tmp_term3 = cohort.Treenumber;
                        double tmp = tmp_term1 * tmp_term2 * tmp_term3 / Math.Pow(EcoregionData.ModelCore.CellLength, 2);
                        siteRD += (float)tmp;
                    }
                }

                SiteVars.SiteRD[cohorts.Site] = siteRD;
            }
            //return siteRD;
        }

        public static void TotalSiteFineFuels(Landis.Library.DensityCohorts.SiteCohorts cohorts)
        {

            double siteFF = 0;
            Landis.Library.DensityCohorts.Cohort.SetSiteAccessFunctions(cohorts);
            if (cohorts == null)
            {
                SiteVars.FineFuels[cohorts.Site] = siteFF;
            }
            else
            {
                foreach (Landis.Library.DensityCohorts.ICohort cohort in cohorts.AllCohorts)
                {
                    siteFF += (double)200.0;
                }

                SiteVars.FineFuels[cohorts.Site] = siteFF;
            }
            //return siteRD;
        }

        public static ISiteVar<float> SiteRD 
        {
            get
            {
                return siteRD;
            }
            
        }

        /// <summary>
        /// Fine Fuels biomass
        /// </summary>
        public static ISiteVar<double> FineFuels
        {
            get
            {
                return fineFuels;
            }
            set
            {
                fineFuels = value;
            }
        }
        //---------------------------------------------------------------------


        //---------------------------------------------------------------------
        //---------------------------------------------------------------------

        public static float TotalSDI(Landis.Library.DensityCohorts.SiteCohorts cohorts)
        {

            float sdi = 0;
            Landis.Library.DensityCohorts.Cohort.SetSiteAccessFunctions(cohorts);
            if (cohorts == null)
            {
                SiteVars.SiteRD[cohorts.Site] = sdi;
            }
            else
            {
                foreach (Landis.Library.DensityCohorts.ICohort cohort in cohorts.AllCohorts)
                {
                    if (SpeciesParameters.SpeciesDensity.AllSpecies[cohort.Species.Index].SpType == 3)
                    {
                        sdi += 0;
                    }
                    else
                    {
                        double tmp_term1 = Math.Pow((cohort.Diameter / 25.4), 1.605);
                        float tmp_term2 = 10000 / SpeciesParameters.SpeciesDensity.AllSpecies[cohort.Species.Index].MaxSDI;
                        int tmp_term3 = cohort.Treenumber;
                        double tmp = tmp_term1 * tmp_term2 * tmp_term3 / Math.Pow(EcoregionData.ModelCore.CellLength, 2);
                        sdi += (float)(cohort.Treenumber * Math.Pow(cohort.Diameter/10, 1.6));
                    }
                }

                
            }
            return sdi;
        }
        //---------------------------------------------------------------------

        public static float SiteSDI(Landis.Library.DensityCohorts.SiteCohorts cohorts)
        {

            float sdi = 0;
            Landis.Library.DensityCohorts.Cohort.SetSiteAccessFunctions(cohorts);
            if (cohorts == null)
            {
                SiteVars.SiteRD[cohorts.Site] = sdi;
            }
            else
            {
                foreach (Landis.Library.DensityCohorts.ICohort cohort in cohorts.AllCohorts)
                {
                    if (SpeciesParameters.SpeciesDensity.AllSpecies[cohort.Species.Index].SpType == 3)
                    {
                        sdi += 0;
                    }
                    else
                    {
                        double tmp_term1 = Math.Pow((cohort.Diameter / 25.4), 1.605);
                        double tmp_term2 = cohort.Treenumber / (Math.Pow(EcoregionData.ModelCore.CellLength, 2) / 10000);
                        double tmp = tmp_term1 * tmp_term2;
                        sdi += (float)tmp;
                    }
                }


            }
            return sdi;
        }
        //---------------------------------------------------------------------

        public static double RelativeBasalArea(ISiteCohorts cohorts, IEnumerable<ISpecies> sppList)
        {
            double ba_numerator = 0;
            double ba_denominator = 0;

            if (cohorts != null)
            {
                foreach (Landis.Library.DensityCohorts.ISpeciesCohorts speciesCohorts in cohorts)
                {
                    double temp_ba = ComputeSpeciesBasal(speciesCohorts);
                    ba_denominator += temp_ba;
                    if (sppList.Contains(speciesCohorts.Species))
                    {
                        ba_numerator += temp_ba;
                    }
                }
            }
            return (ba_numerator / ba_denominator);
        }


        //---------------------------------------------------------------------

        public static double ComputeSpeciesBasal(Landis.Library.DensityCohorts.ISpeciesCohorts cohorts)
        {
            double local_const = 3.1415926 / (4 * 10000.00);
            double total = 0;
            if (cohorts != null)
                total = cohorts.Sum(x => Math.Pow(x.Diameter, 2) * local_const * x.Treenumber);
            return total;
        }

        //---------------------------------------------------------------------

        public static int ComputeSpeciesDensity(Landis.Library.DensityCohorts.ISpeciesCohorts cohorts)
        {
            int total = 0;
            if (cohorts != null)
                total = cohorts.Sum(x => x.Treenumber);
            return total;
        }

        //---------------------------------------------------------------------

        public static double SpeciesQMD(ISiteCohorts cohorts, IEnumerable<ISpecies> sppList)
        {
            double temp_ba = 0;
            int temp_tph = 0;
            if (cohorts != null)
                foreach (Landis.Library.DensityCohorts.ISpeciesCohorts speciesCohorts in cohorts)
                {
                    if (sppList.Contains(speciesCohorts.Species))
                    {
                        temp_ba += ComputeSpeciesBasal(speciesCohorts);
                        temp_tph += ComputeSpeciesDensity(speciesCohorts);
                    }
                }
            if (temp_ba == 0 && temp_tph == 0)
            {
                return 0;
            }
            else
            {
                return (float)Math.Sqrt((temp_ba / temp_tph) / 0.00007854);
            }
        }
    }
}
