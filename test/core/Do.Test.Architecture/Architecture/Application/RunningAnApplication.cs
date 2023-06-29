namespace Do.Test.Architecture.Application;

public class RunningAnApplication : Spec
{
    [Test]
    public void Application_uses_layers_to_get_its_phases_configured()
    {
        /*
        var build = GiveMe.ABuild();
        var phase = MockMe.APhase();
        var layer = MockMe.ALayer(thatConfigures: phase);

        var app = build.As(app =>
        {
            app.Layers.Add(layer);
        });

        app.Run();
        */

        Assert.Fail("not implemented");
    }

    [Test]
    [Ignore("not implemented")]
    public void Each_phase_is_applied_separately_to_all_layers() => Assert.Fail();

    [Test]
    [Ignore("not implemented")]
    public void Application_provides_layers_with_a_run_context_that_layers_can_add_objects_to_or_get_objects_from() => Assert.Fail();

    // e.g. context.Has<ApplicationBuilder>() && context.Had<IServicesCollection>();
    [Test]
    [Ignore("not implemented")]
    public void Application_resolves_which_phase_to_initialize_automatically_by_checking_if_context_is_ready_for_them() => Assert.Fail();

    [Test]
    [Ignore("not implemented")]
    public void Application_initializes_each_phase_before_they_are_applied_to_layers() => Assert.Fail();

    [Test]
    [Ignore("not implemented")]
    public void Application_disposes_each_phase_after_they_are_applied() => Assert.Fail();
}
