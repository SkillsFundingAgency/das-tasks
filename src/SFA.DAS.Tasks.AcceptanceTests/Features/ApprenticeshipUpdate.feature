@AML1411
Feature: ApprenticeshipUpdate

Scenario: Apprentice Changes To Review
Given I have an Apprenticeship Changes To Review
When apprenticeship_update_created message get publish
Then I should have a ApprenticeChangesToReview Task

Scenario: Apprentice Changes Accepted
Given I have an Apprenticeship Accepted
When apprenticeship_update_accepted message get publish
Then ApprenticeChangesToReview Task should be removed

Scenario: Apprentice Changes Rejected
Given I have an Apprenticeship Rejected
When apprenticeship_update_rejected message get publish
Then ApprenticeChangesToReview Task should be removed

Scenario: Apprentice Changes Cancelled
Given I have an Apprenticeship Cancelled
When apprenticeship_update_cancelled message get publish
Then ApprenticeChangesToReview Task should be removed