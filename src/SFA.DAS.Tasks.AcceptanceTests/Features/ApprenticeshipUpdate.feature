@AML1411
Feature: ApprenticeshipUpdate

Scenario: 020 Apprentice Changes To Review
Given I have an Apprenticeship Changes To Review
When apprenticeship_update_created message get publish
Then I should have a ApprenticeChangesToReview Task

Scenario: 021 Apprentice Changes Accepted
Given I have an Apprenticeship Accepted
When apprenticeship_update_accepted message get publish
Then ApprenticeChangesToReview Task should be removed

Scenario: 022 Apprentice Changes Rejected
Given I have an Apprenticeship Rejected
When apprenticeship_update_rejected message get publish
Then ApprenticeChangesToReview Task should be removed

Scenario: 023 Apprentice Changes Cancelled
Given I have an Apprenticeship Cancelled
When apprenticeship_update_cancelled message get publish
Then ApprenticeChangesToReview Task should be removed