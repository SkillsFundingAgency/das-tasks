using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Tasks.Application.Validation;

namespace SFA.DAS.Tasks.Application.UnitTests
{
    public static class ExtensionMethods
    {
        public static void VerifyResultIsFalse(this ValidationResult result, string expectedValidationMessage, string actualValidationMessage)
        {
            Assert.IsFalse(result.IsValid());
            Assert.AreEqual(expectedValidationMessage, actualValidationMessage);
        }
    }
}
