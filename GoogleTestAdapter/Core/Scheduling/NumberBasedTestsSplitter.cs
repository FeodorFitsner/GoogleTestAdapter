﻿using System;
using System.Collections.Generic;
using System.Linq;
using GoogleTestAdapter.Helpers;
using GoogleTestAdapter.Model;

namespace GoogleTestAdapter.Scheduling
{
    public class NumberBasedTestsSplitter : ITestsSplitter
    {
        private IEnumerable<TestCase> TestcasesToRun { get; }
        private TestEnvironment TestEnvironment { get; }


        public NumberBasedTestsSplitter(IEnumerable<TestCase> testcasesToRun, TestEnvironment testEnvironment)
        {
            this.TestEnvironment = testEnvironment;
            this.TestcasesToRun = testcasesToRun;
        }


        public List<List<TestCase>> SplitTestcases()
        {
            int nrOfThreadsToUse = Math.Min(TestEnvironment.Options.MaxNrOfThreads, TestcasesToRun.Count());
            List<TestCase>[] splitTestCases = new List<TestCase>[nrOfThreadsToUse];
            for (int i = 0; i < nrOfThreadsToUse; i++)
            {
                splitTestCases[i] = new List<TestCase>();
            }

            int testcaseCounter = 0;
            foreach (TestCase testCase in TestcasesToRun)
            {
                splitTestCases[testcaseCounter++ % nrOfThreadsToUse].Add(testCase);
            }

            return splitTestCases.ToList();
        }

    }

}