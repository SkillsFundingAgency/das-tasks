﻿@AML1412
Feature: CohortApproved


Scenario: Cohort Approval Requested By Provider
Given I have Cohort Ready For Approval
When cohort_approval_requested_by_provider message get publish
Then I should have a CohortRequestReadyForApproval Task


Scenario: Cohort Approved By Employer
Given I have Approved A Cohort
When cohort_approved_by_employer message get publish
Then CohortRequestReadyForApproval Task should be removed