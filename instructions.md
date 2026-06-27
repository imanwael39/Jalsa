# Jalsa AI Development Instructions

## Objective

Build the project incrementally.

Never attempt to implement an entire sprint or the entire MVP in a single execution.

The project must always remain in a working state.

---

# Source Documents

Treat these files as the single source of truth.

Read them completely before beginning work.

Priority:

1. MVP-EXECUTION-PLAN.md
2. Sprint1.md
3. Sprint2.md
4. Sprint3.md
5. Sprint4.md
6. SRS
7. Existing project codebase

If any document conflicts with the existing implementation, analyze the code first and explain the conflict before changing anything.

---

# General Workflow

Never start multiple unrelated tasks.

Always execute:

Read
↓

Analyze

↓

Implement ONE task

↓

Test

↓

Fix issues

↓

Retest

↓

Commit

↓

Report

↓

STOP

Wait for approval before continuing.

---

# Sprint Workflow

When beginning a sprint:

Create a branch.

Naming convention:

feature/sprint-1
feature/sprint-2
feature/sprint-3
feature/sprint-4

Never work directly on:

- main
- master
- develop

unless explicitly instructed.

---

# Task Workflow

Inside every sprint:

Only work on ONE task at a time.

Example:

Sprint 1

BE-AUTH-02

↓

Commit

↓

BE-EXE-01

↓

Commit

↓

BE-SES-01

↓

Commit

↓

Continue until Sprint Done.

Never implement multiple task IDs together unless they are impossible to separate.

---

# Before Starting Any Task

Understand:

- dependencies
- affected modules
- existing architecture
- current implementation
- DTOs
- services
- repositories
- controllers
- frontend integration

Never duplicate existing code.

Always reuse existing architecture.

---

# Implementation Rules

Follow Clean Architecture.

Respect existing:

- naming
- folder structure
- dependency injection
- repository pattern
- service pattern
- DTO pattern
- authorization
- validation
- error handling

Never introduce shortcuts.

Never hardcode values.

Never leave TODO placeholders.

Never leave partially implemented methods.

---

# Git Rules

Before every task:

Create or switch to the correct sprint branch.

After every completed task:

Create a commit.

Commit format:

[TASK-ID] Short description

Examples

[BE-SES-01] Create Session DTOs

[FE-SES-02] Connect SessionService

One task = One commit.

Never combine multiple unrelated tasks into one commit.

---

# Testing Rules

Every task MUST be tested.

Testing order:

1. Build
2. Restore packages
3. Run backend
4. Run frontend
5. Execute related unit tests
6. Execute related integration tests
7. Manual verification
8. Verify no regression

If tests fail:

Fix them before continuing.

Never continue with failing tests.

---

# Backend Testing

Verify:

- API endpoints
- authorization
- validation
- response codes
- database updates
- migrations
- logging
- exception handling

Use Swagger or API tests.

---

# Frontend Testing

Verify:

- routing
- forms
- validation
- API integration
- loading states
- error states
- success messages
- responsive layout

Ensure no console errors.

---

# Sprint Validation

When all sprint tasks are complete:

Run complete sprint validation.

Include:

Backend build

Frontend build

Application startup

API verification

Navigation testing

Regression testing

Integration testing

Database verification

Only mark the sprint complete if every task passes.

---

# Progress Report

After every task provide a report.

Format:

---------------------------------

Current Sprint

Sprint X

Current Task

TASK-ID

Status

Completed

Files Modified

- ...

Tests Executed

- ...

Result

PASS / FAIL

Commit

commit hash

Remaining Tasks

- TASK
- TASK
- TASK

Sprint Progress

3 / 8 Tasks Complete

Known Issues

...

Next Recommended Task

TASK-ID

STOP

Wait for approval.

---------------------------------

---

# Failure Policy

If blocked:

Do not guess.

Investigate.

Explain:

- problem
- cause
- possible solutions

Then stop.

---

# Completion Rules

Never declare a task complete unless:

✓ Build succeeds

✓ Tests pass

✓ Feature works

✓ No regression found

✓ Commit created

✓ Progress report generated

---

# Completion of Sprint

At sprint completion provide:

Completed Tasks

Remaining MVP

Test Results

Coverage Summary

Known Technical Debt

Risk Assessment

Readiness Score

Only then proceed to the next sprint.

---

# Completion of MVP

After Sprint 4:

Perform full end-to-end validation.

Test every MVP workflow.

Generate:

- MVP Completion Report
- Remaining Technical Debt
- Bugs Found
- Suggested Improvements
- Deployment Checklist
- Final Readiness Score

Do not mark MVP complete unless every required MVP feature is verified.