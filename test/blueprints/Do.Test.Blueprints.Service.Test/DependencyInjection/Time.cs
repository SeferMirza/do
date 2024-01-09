﻿namespace Do.Test.DependencyInjection;

public class Time : TestServiceSpec
{
    [Test]
    public void TimeProvider_is_injected_to_access_machine_time()
    {
        MockMe.TheTime(now: GiveMe.ADateTime(year: 2023, month: 11, day: 29));
        var singleton = GiveMe.The<Singleton>();

        var actual = singleton.GetNow();

        actual.ShouldBe(GiveMe.ADateTime(year: 2023, month: 11, day: 29));
    }
}
