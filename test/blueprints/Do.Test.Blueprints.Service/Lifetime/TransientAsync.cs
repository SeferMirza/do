﻿namespace Do.Test.Lifetime;

public class TransientAsync
{
    internal Task<TransientAsync> With() =>
        Task.FromResult(this);
}