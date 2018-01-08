﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.2.0.0
//      SpecFlow Generator Version:2.2.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace SFA.DAS.Tasks.AcceptanceTests.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.2.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("SignAnAgreement")]
    public partial class SignAnAgreementFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "SignAnAgreement.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "SignAnAgreement", null, ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("010 Sign an Agreement")]
        [NUnit.Framework.CategoryAttribute("AML1239")]
        public virtual void _010SignAnAgreement()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("010 Sign an Agreement", new string[] {
                        "AML1239"});
#line 4
this.ScenarioSetup(scenarioInfo);
#line 5
testRunner.Given("I Sign an agreement", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 6
testRunner.When("agreement_signed message get publish", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 7
testRunner.Then("I should have a AddApprentices Task", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 8
testRunner.And("AgreementToSign Task should be removed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("011 Create Draft Cohort")]
        [NUnit.Framework.CategoryAttribute("AML1415")]
        public virtual void _011CreateDraftCohort()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("011 Create Draft Cohort", new string[] {
                        "AML1415"});
#line 11
this.ScenarioSetup(scenarioInfo);
#line 12
testRunner.Given("I Create Draft Cohort", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 13
testRunner.When("cohort_created message get publish", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 14
testRunner.Then("AddApprentices Task should be removed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("012 Create Draft Cohort And Sign An Agreement")]
        [NUnit.Framework.CategoryAttribute("AML1415")]
        public virtual void _012CreateDraftCohortAndSignAnAgreement()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("012 Create Draft Cohort And Sign An Agreement", new string[] {
                        "AML1415"});
#line 17
this.ScenarioSetup(scenarioInfo);
#line 18
testRunner.Given("I have Draft Cohort", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 19
testRunner.When("agreement_signed_cohort_created message get publish", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 20
testRunner.Then("AddApprentices Task should not be added", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
