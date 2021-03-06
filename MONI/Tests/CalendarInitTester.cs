﻿using System;
using System.Collections.Generic;
using MONI.Data;
using NUnit.Framework;
using System.Linq;

namespace MONI.Tests {
  public class CalendarInitTester {
    [Test]
    public void WorkYear_ctor_CreateWholeYear() {
      WorkYear wy = new WorkYear(2011, new List<ShortCut>(), 0, 1, null, null);
      Assert.AreEqual(12, wy.Months.Count);
      //test sample month
      var march = wy.Months.ElementAt(2);
      Assert.AreEqual(5, march.Weeks.Count);
      Assert.AreEqual(31, march.Weeks.Sum(w => w.Days.Count()));
      Assert.AreEqual(DayOfWeek.Tuesday, march.Weeks.First().Days.First().DayOfWeek);
    }
  }
}