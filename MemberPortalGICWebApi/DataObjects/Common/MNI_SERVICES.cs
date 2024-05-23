using MemberPortalGICWebApi.CoverageBalanceQuery;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.DataObjects
{
    public class MNI_SERVICES
    {
        public List<PalnsAll> CustomTOGetBenefitsAndPlans(long policynumber, long membernumber)
        {
            #region old code
            //coverageBalancesQueryClient client1 = new coverageBalancesQueryClient("coverageBalancesQuerySoapHttpPort");

            //CoverageBalancesQueryUser_coveragebalancesquery_Out resulw = client1.coveragebalancesquery(policynumber, membernumber, null, null, null);

            //List<WsCoverageBalancesOutRecUser> NEW = new List<WsCoverageBalancesOutRecUser>();
            //if (resulw.statusoutputrecOut.statusCode == 0)
            //{


            //    var tt = resulw.coveragebalancesouttabOut.GroupBy(m => m.planNumber)
            //                     .Where(g => g.Select(u => u.planNumber).Distinct().Count() > 1)
            //                     // .Select(g => g.Key)
            //                     .ToList();
            //    IEnumerable<IGrouping<Decimal?, WsCoverageBalancesOutRecUser>> groups = resulw.coveragebalancesouttabOut.GroupBy(m => m.planNumber);
            //    IEnumerable<WsCoverageBalancesOutRecUser> smths = groups.SelectMany(group => group);
            //    List<WsCoverageBalancesOutRecUser> newList = smths.ToList();
            //    List<WsCoverageBalancesOutRecUser> Sortedlis3t = new List<WsCoverageBalancesOutRecUser>();
            //    IEnumerable<WsCoverageBalancesOutRecUser> aasasa;
            //    var t = newList;

            //    var ndd = t.Where(x => x.groupingTab[0].criterionTab[0].accumulatorTab[0].appliedLevelId == "03");
            //    var p = t.OrderByDescending(m => m.insuranceCompanyNumber);

            //    int dd = 0;
            //    List<PalnsAll> plannumberss = new List<PalnsAll>();
            //    foreach (var gt in t)
            //    {
            //        foreach (var cf in gt.groupingTab)
            //        {
            //            var tre = cf.criterionTab.Where(x => x.ruleTab[0].appliedLevelId == "02")
            //                 .Where(x => x.ruleTab[0].ruleTypeId == "02")
            //                 .Where(x => x.ruleTab[0].limitRuleRec.limitTypeId == "01"
            //                 || x.ruleTab[0].limitRuleRec.limitTypeId == "03"
            //                 || x.ruleTab[0].limitRuleRec.limitTypeId == "04").ToList();
            //            if (tre.Count > 0)
            //            {
            //                dd++;
            //                plannumberss.Add(new PalnsAll
            //                {
            //                    PlanNumber = Convert.ToDecimal(gt.planNumber),
            //                    PlanDescrptions = gt.planDescription,
            //                    TotlaAmount = Convert.ToDecimal(tre[0].ruleTab[0].limitRuleRec.ruleDefinitionAmtRec.amount),
            //                    UsedAmount = Convert.ToDecimal(tre[0].ruleTab[0].limitRuleRec.ruleBalanceAmtRec.usedAmount)

            //                });
            //            }
            //        }
            //    }

            //    List<PalnsAll> plannumbersDistinct = new List<PalnsAll>();
            //    //plannumbersDistinct = plannumberss.GroupBy(customer => customer.PlanNumber).Select(group => group.First()).ToList();
            //    plannumbersDistinct = plannumberss.GroupBy(customer => customer.PlanNumber).Select(group => group.Where(x => x.TotlaAmount >0).First()).ToList();

            //    foreach (var Eachplandistint in plannumbersDistinct)
            //    {

            //        var EachContributed = t.Where(d => d.planNumber == Eachplandistint.PlanNumber).ToList();
            //        List<Benefits> benifitslistadded = new List<Benefits>();
            //        foreach (var elemnts in EachContributed)
            //        {


            //            foreach (var cf in elemnts.groupingTab)
            //            {

            //                var fr = cf.criterionTab.Where(x => x.ruleTab[0].appliedLevelId == "03" || x.ruleTab[0].appliedLevelId == "01");
            //                int frt = fr.Count();
            //                var tre = cf.criterionTab.Where(x => x.ruleTab[0].appliedLevelId == "03")
            //                       .Where(x => x.ruleTab[0].ruleTypeId == "02")
            //                       .Where(x => x.ruleTab[0].limitRuleRec.limitTypeId == "01"

            //                     || x.ruleTab[0].limitRuleRec.limitTypeId == "02"
            //                      || x.ruleTab[0].limitRuleRec.limitTypeId == "03"
            //                       || x.ruleTab[0].limitRuleRec.limitTypeId == "04").ToList();
            //                if (tre.Count > 0)
            //                {
            //                    dd++;

            //                    Benefits bendfits = new Benefits();
            //                    bendfits.BenefitNumber = Convert.ToDecimal(elemnts.benefitNumber);
            //                    bendfits.BenefitDescrptions = elemnts.benefitDescription;
            //                    if (elemnts.benefitNumber.ToString() == "3002")
            //                    {

            //                    }
            //                    // if (tre[0].ruleTab[0].limitRuleRec.ruleDefinitionAmtRec!=null)
            //                    if (tre[0].ruleTab[0].limitRuleRec.ruleBalanceQtyRec != null)
            //                        bendfits.TotlaAmount = Convert.ToDecimal(tre[0].ruleTab[0].limitRuleRec.ruleBalanceQtyRec.remainingQuantity);
            //                    // bendfits.TotlaAmount = Convert.ToDecimal(tre[0].ruleTab[0].limitRuleRec.ruleDefinitionAmtRec.amount);

            //                    //  if (tre[0].ruleTab[0].limitRuleRec.ruleBalanceAmtRec != null)
            //                    if (tre[0].ruleTab[0].limitRuleRec.ruleBalanceQtyRec != null)
            //                        // bendfits.UsedAmount = Convert.ToDecimal(tre[0].ruleTab[0].limitRuleRec.ruleBalanceAmtRec.usedAmount);
            //                        bendfits.UsedAmount = Convert.ToDecimal(tre[0].ruleTab[0].limitRuleRec.ruleBalanceQtyRec.usedQuantity);

            //                    //benifitslistadded.Add(new Benefits
            //                    //{
            //                    //    BenefitNumber = Convert.ToDecimal(elemnts.benefitNumber),
            //                    //    BenefitDescrptions = elemnts.benefitDescription,

            //                    //   // TotlaAmount = Convert.ToDecimal(tre[0].ruleTab[0].limitRuleRec.ruleDefinitionAmtRec.amount),
            //                    //   // UsedAmount = Convert.ToDecimal(tre[0].ruleTab[0].limitRuleRec.ruleBalanceAmtRec.usedAmount)

            //                    //});

            //                    benifitslistadded.Add(bendfits);

            //                }


            //            }
            //            Eachplandistint.BenefitsAdded = benifitslistadded;
            //        }

            //    }


            //    var r = dd;




            //    return plannumbersDistinct;
            //}
            //else
            //{
            //    return null;
            //}
            #endregion

            try
            {
                coverageBalancesQueryClient client1 = new coverageBalancesQueryClient("coverageBalancesQuerySoapHttpPort");

                CoverageBalancesQueryUser_coveragebalancesquery_Out resulw = client1.coveragebalancesquery(policynumber, membernumber, null, null, null,null);

                List<WsCoverageBalancesOutRecUser> NEW = new List<WsCoverageBalancesOutRecUser>();
                if (resulw.statusoutputrecOut.statusCode == 0)
                {


                    var tt = resulw.coveragebalancesouttabOut.GroupBy(m => m.planNumber)
                                     .Where(g => g.Select(u => u.planNumber).Distinct().Count() > 1)
                                     // .Select(g => g.Key)
                                     .ToList();
                    IEnumerable<IGrouping<Decimal?, WsCoverageBalancesOutRecUser>> groups = resulw.coveragebalancesouttabOut.GroupBy(m => m.planNumber);
                    IEnumerable<WsCoverageBalancesOutRecUser> smths = groups.SelectMany(group => group);
                    List<WsCoverageBalancesOutRecUser> newList = smths.ToList();
                    List<WsCoverageBalancesOutRecUser> Sortedlis3t = new List<WsCoverageBalancesOutRecUser>();
                    IEnumerable<WsCoverageBalancesOutRecUser> aasasa;
                    var t = newList;

                    var ndd = t.Where(x => x.groupingTab[0].criterionTab[0].accumulatorTab[0].appliedLevelId == "03");
                    var p = t.OrderByDescending(m => m.insuranceCompanyNumber);

                    int dd = 0;
                    List<PalnsAll> plannumberss = new List<PalnsAll>();
                    foreach (var gt in t)
                    {
                        try
                        {

                            foreach (var cf in gt.groupingTab)
                            {
                                //var tre = cf.criterionTab.Where(x => x.ruleTab[0].appliedLevelId == "02")
                                //     .Where(x => x.ruleTab[0].ruleTypeId == "02")
                                //     .Where(x => x.ruleTab[0].limitRuleRec.limitTypeId == "01"
                                //     || x.ruleTab[0].limitRuleRec.limitTypeId == "03"
                                //     || x.ruleTab[0].limitRuleRec.limitTypeId == "04").ToList();

                                var tre = cf.criterionTab.ToList();
                                if (tre.Count > 0)
                                {
                                    dd++;


                                    if (tre[0].ruleTab != null)
                                    {
                                        if (tre[0].ruleTab[0].limitRuleRec != null)
                                        {
                                            plannumberss.Add(new PalnsAll
                                            {
                                                PlanNumber = Convert.ToDecimal(gt.planNumber),
                                                PlanDescrptions = gt.planDescription,
                                                TotlaAmounts = Convert.ToDecimal(tre[0].ruleTab[0].limitRuleRec.ruleDefinitionAmtRec == null ? 0 : tre[0].ruleTab[0].limitRuleRec.ruleDefinitionAmtRec.amount),
                                                UsedAmounts = Convert.ToDecimal(tre[0].ruleTab[0].limitRuleRec.ruleBalanceAmtRec == null ? 0 : tre[0].ruleTab[0].limitRuleRec.ruleBalanceAmtRec.usedAmount)

                                            });

                                        }
                                        else
                                        {
                                            plannumberss.Add(new PalnsAll
                                            {
                                                PlanNumber = Convert.ToDecimal(gt.planNumber),
                                                PlanDescrptions = gt.planDescription,
                                                TotlaAmounts =0,
                                                UsedAmounts = 0


                                            });

                                        }

                                        //plannumberss.Add(new PalnsAll
                                        //{
                                        //    PlanNumber = Convert.ToDecimal(gt.planNumber),
                                        //    PlanDescrptions = gt.planDescription,
                                        //    TotlaAmounts = Convert.ToDecimal(tre[0].ruleTab[0].limitRuleRec.ruleDefinitionAmtRec == null ? 0 : tre[0].ruleTab[0].limitRuleRec.ruleDefinitionAmtRec.amount),
                                        //    UsedAmounts = Convert.ToDecimal(tre[0].ruleTab[0].limitRuleRec.ruleBalanceAmtRec == null ? 0 : tre[0].ruleTab[0].limitRuleRec.ruleBalanceAmtRec.usedAmount)

                                        //});
                                    }
                                    else
                                    {

                                        plannumberss.Add(new PalnsAll
                                        {
                                            PlanNumber = Convert.ToDecimal(gt.planNumber),
                                            PlanDescrptions = gt.planDescription,
                                            TotlaAmounts = 0,
                                            UsedAmounts = 0

                                        });
                                    }

                                    //plannumberss.Add(new PalnsAll
                                    //{
                                    //    PlanNumber = Convert.ToDecimal(gt.planNumber),
                                    //    PlanDescrptions = gt.planDescription,
                                    //    TotlaAmount = Convert.ToDecimal(tre[0].ruleTab[0].limitRuleRec.ruleDefinitionAmtRec == null ? 0 : tre[0].ruleTab[0].limitRuleRec.ruleDefinitionAmtRec.amount),
                                    //    UsedAmount = Convert.ToDecimal(tre[0].ruleTab[0].limitRuleRec.ruleBalanceAmtRec == null ? 0 : tre[0].ruleTab[0].limitRuleRec.ruleBalanceAmtRec.usedAmount)

                                    //});
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                            throw ex;
                        }

                    }

                    List<PalnsAll> plannumbersDistinct = new List<PalnsAll>();
                    //  plannumbersDistinct = plannumberss.GroupBy(customer => customer.PlanNumber).Select(group => group.Where(x => x.TotlaAmount > 0).First()).ToList();

                    //Changed changed >= condition for TotlaAmount as exception was coming when amount = 0 for some plan 
                    plannumbersDistinct = plannumberss.GroupBy(customer => customer.PlanNumber).Select(group => group.Where(x => x.TotlaAmounts > 0).Count() > 0 ? group.Where(x => x.TotlaAmounts > 0).First() : group.Where(x => x.TotlaAmounts == 0).First()).ToList();

                    foreach (var Eachplandistint in plannumbersDistinct)
                    {

                        var EachContributed = t.Where(d => d.planNumber == Eachplandistint.PlanNumber).ToList();
                        List<Benefits> benifitslistadded = new List<Benefits>();
                        foreach (var elemnts in EachContributed)
                        {


                            foreach (var cf in elemnts.groupingTab)
                            {
                                if (cf.criterionTab[0].ruleTab != null)
                                {

                                    var fr = cf.criterionTab.Where(x => x.ruleTab[0].appliedLevelId == "03" || x.ruleTab[0].appliedLevelId == "01");
                                    int frt = fr.Count();
                                    var tre = cf.criterionTab.Where(x => x.ruleTab[0].appliedLevelId == "03")
                                           .Where(x => x.ruleTab[0].ruleTypeId == "02")
                                           .Where(x => x.ruleTab[0].limitRuleRec.limitTypeId == "01"

                                         || x.ruleTab[0].limitRuleRec.limitTypeId == "02"
                                          || x.ruleTab[0].limitRuleRec.limitTypeId == "03"
                                           || x.ruleTab[0].limitRuleRec.limitTypeId == "04").ToList();
                                    if (tre.Count > 0)
                                    {
                                        dd++;

                                        Benefits bendfits = new Benefits();
                                        bendfits.BenefitNumber = Convert.ToDecimal(elemnts.benefitNumber);
                                        bendfits.BenefitDescrptions = elemnts.benefitDescription;
                                        if (elemnts.benefitNumber.ToString() == "3002")
                                        {

                                        }
                                        // if (tre[0].ruleTab[0].limitRuleRec.ruleDefinitionAmtRec!=null)
                                        if (tre[0].ruleTab[0].limitRuleRec.ruleBalanceQtyRec != null)
                                            bendfits.TotlaAmount = Convert.ToDecimal(tre[0].ruleTab[0].limitRuleRec.ruleBalanceQtyRec.remainingQuantity);
                                        // bendfits.TotlaAmount = Convert.ToDecimal(tre[0].ruleTab[0].limitRuleRec.ruleDefinitionAmtRec.amount);

                                        //  if (tre[0].ruleTab[0].limitRuleRec.ruleBalanceAmtRec != null)
                                        if (tre[0].ruleTab[0].limitRuleRec.ruleBalanceQtyRec != null)
                                            // bendfits.UsedAmount = Convert.ToDecimal(tre[0].ruleTab[0].limitRuleRec.ruleBalanceAmtRec.usedAmount);
                                            bendfits.UsedAmount = Convert.ToDecimal(tre[0].ruleTab[0].limitRuleRec.ruleBalanceQtyRec.usedQuantity);

                                        //benifitslistadded.Add(new Benefits
                                        //{
                                        //    BenefitNumber = Convert.ToDecimal(elemnts.benefitNumber),
                                        //    BenefitDescrptions = elemnts.benefitDescription,

                                        //   // TotlaAmount = Convert.ToDecimal(tre[0].ruleTab[0].limitRuleRec.ruleDefinitionAmtRec.amount),
                                        //   // UsedAmount = Convert.ToDecimal(tre[0].ruleTab[0].limitRuleRec.ruleBalanceAmtRec.usedAmount)

                                        //});

                                        benifitslistadded.Add(bendfits);

                                    }
                                }


                            }
                            Eachplandistint.BenefitsAdded = benifitslistadded;
                        }

                    }


                    var r = dd;
                    return plannumbersDistinct;
                }
                else
                {
                    return null;
                }
            }

            catch (Exception ex)
            {
                return null;
            }



        }

    }
    public class PalnsAll
    {
        public decimal PlanNumber { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,##0.000#}")]
        public decimal TotlaAmounts { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,##0.000#}")]
        public decimal UsedAmounts { get; set; }
        public string PlanDescrptions { get; set; }

        public string PlanDescrptionsAR { get; set; }

        public string TotlaAmount { get; set; }
        public string UsedAmount { get; set; }
        public List<Benefits> BenefitsAdded { get; set; }
    }
    public class Benefits
    {
        public decimal BenefitNumber { get; set; }
        // [DisplayFormat(DataFormatString = "{0:#,##0.000#}")]
        public decimal TotlaAmount { get; set; }
        // [DisplayFormat(DataFormatString = "{0:#,##0.000#}")]
        public decimal UsedAmount { get; set; }
        public string BenefitDescrptions { get; set; }
    }
}
