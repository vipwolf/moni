﻿using System;
using System.Collections.Generic;
using System.IO;
using MONI.Data;
using NUnit.Framework;
using Newtonsoft.Json;

namespace MONI.Tests
{
  [TestFixture]
  public class SettingsTester
  {
    [Test]
    public void WriteJson() {
      var parserSettings = new WorkDayParserSettings();
      parserSettings.ShortCuts.Add(new ShortCut("ctbn", "25482-420(features)"));
      parserSettings.ShortCuts.Add(new ShortCut("ctbp", "25482-811(performanceverbesserungen)"));
      parserSettings.ShortCuts.Add(new ShortCut("ctbf", "25482-811(tracker)"));
      parserSettings.ShortCuts.Add(new ShortCut("ctbm", "25482-140(meeting)"));
      parserSettings.ShortCuts.Add(new ShortCut("ctbr", "25482-050(ac-hh-ac)"));
      parserSettings.ShortCuts.Add(new ShortCut("ktln", "25710-420(feature)"));
      parserSettings.ShortCuts.Add(new ShortCut("ktlf", "25710-811(tracker)"));
      parserSettings.ShortCuts.Add(new ShortCut("ktlm", "25710-140(meeting)"));
      parserSettings.ShortCuts.Add(new ShortCut("ktlr", "25710-050(reise)"));
      parserSettings.ShortCuts.Add(new ShortCut("u", "20030-000(urlaub)"));
      parserSettings.ShortCuts.Add(new ShortCut("krank", "20020-000(krank/doc)"));
      parserSettings.ShortCuts.Add(new ShortCut("tm", "20018-140(terminalmeeting)"));
      parserSettings.ShortCuts.Add(new ShortCut("mm", "20018-140(tess/monatsmeeting)"));
      parserSettings.ShortCuts.Add(new ShortCut("swe", "20308-000(swe projekt)"));
      parserSettings.ShortCuts.Add(new ShortCut("jmb", "20308-000(jean-marie ausbildungsbetreuung)"));
      parserSettings.ShortCuts.Add(new ShortCut("w", "20180-000(weiterbildung)"));
      parserSettings.InsertDayBreak = true;
      parserSettings.DayBreakTime = new TimeItem(12);
      parserSettings.DayBreakDurationInMinutes = 30;

      var mainSettings = new MainSettings();

      MoniSettings ms = new MoniSettings();
      ms.ParserSettings = parserSettings;
      ms.MainSettings = mainSettings;

      var serializeObject = JsonConvert.SerializeObject(ms, Formatting.Indented);
      File.WriteAllText("settings.json.test", serializeObject);
    }

    [Test]
    public void GetValidShortCuts_NoDoubles_ReturnListAsIs() {
      var shortCuts = new List<ShortCut>();
      shortCuts.Add(new ShortCut("ctbn", "12345-000"));
      shortCuts.Add(new ShortCut("ctbp", "12345-000"));
      shortCuts.Add(new ShortCut("ctbf", "12345-000"));

      var doesntMatter = DateTime.Now;
      var validShortCuts = WorkDayParserSettings.ValidShortCuts(shortCuts, doesntMatter);
      CollectionAssert.AreEqual(shortCuts, validShortCuts);
    }

    [Test]
    public void GetValidShortCuts_MultipleKeysInterval_ReturnJustRightShortcuts() {
      var shortCuts = new List<ShortCut>();
      shortCuts.Add(new ShortCut("ctbn", "12345-000"));
      var findMe = new ShortCut("ctbn", "54321-000", new DateTime(2000,1,1));
      shortCuts.Add(findMe);
      var andMe = new ShortCut("ktln", "25710-420(feature)");
      shortCuts.Add(andMe);

      var matchOnShortcut = new DateTime(2005,1,1);
      var validShortCuts = WorkDayParserSettings.ValidShortCuts(shortCuts, matchOnShortcut);
      CollectionAssert.AreEqual(new[] { findMe, andMe }, validShortCuts);
    }

    [Test]
    public void GetValidShortCuts_MultipleKeysDateMatch_ReturnJustRightShortcuts() {
      var shortCuts = new List<ShortCut>();
      shortCuts.Add(new ShortCut("ctbn", "12345-000"));
      var matchDate = new DateTime(2000, 1, 1);
      var findMe = new ShortCut("ctbn", "54321-000", matchDate);
      shortCuts.Add(findMe);
      var andMe = new ShortCut("ktln", "25710-420(feature)");
      shortCuts.Add(andMe);

      var validShortCuts = WorkDayParserSettings.ValidShortCuts(shortCuts, matchDate);
      CollectionAssert.AreEqual(new[] { findMe, andMe }, validShortCuts);
    }

    [Test]
    public void GetValidShortCuts_NoMatch_ReturnNoShortcuts() {
      var shortCuts = new List<ShortCut>();
      shortCuts.Add(new ShortCut("ctbn", "54321-000", new DateTime(2001, 1, 1)));
      shortCuts.Add(new ShortCut("ctbn", "54321-000", new DateTime(2002, 1, 1)));
      shortCuts.Add(new ShortCut("ctbn", "54321-000", new DateTime(2003, 1, 1)));

      var validShortCuts = WorkDayParserSettings.ValidShortCuts(shortCuts, new DateTime(2000, 1, 1));
      CollectionAssert.IsEmpty(validShortCuts);
    }
     
  }
}