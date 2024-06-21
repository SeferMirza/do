﻿using Baked.Branding;
using Baked.Testing;

namespace Baked.Test;

public static class BannerExtensions
{
    public static IBanner ABanner(this Mocker _) =>
        new Mock<IBanner>().Object;

    public static void VerifyPrinted(this IBanner banner) =>
        Mock.Get(banner).Verify(b => b.Print());

    public static BakedBanner ABakedBanner(this Stubber _) =>
        new();
}