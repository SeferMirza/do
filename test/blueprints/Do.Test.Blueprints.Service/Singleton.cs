﻿using Do.Core;
using Do.Database;

namespace Do.Test;

public class Singleton
{
    readonly ISystem _system;
    readonly Func<Entity> _newEntity;
    readonly ITransaction _transaction;

    public Singleton(ISystem system, Func<Entity> newEntity, ITransaction transaction) =>
        (_system, _newEntity, _transaction) = (system, newEntity, transaction);

    public DateTime GetNow() => _system.Now;

    public async Task TestTransactionAction()
    {
        await _transaction.CommitAsync(() =>
        {
            // do not remove this variable, this is to ensure call is made to `Action` overload
            var _ = _newEntity().With(Guid.NewGuid(), "test", "transaction action", 1, new("https://action.com"), new { transaction = "action" }, Status.Enabled);
        });

        throw new Exception();
    }

    public async Task TestTransactionFunc()
    {
        var entity = await _transaction.CommitAsync(() =>
            _newEntity().With(Guid.NewGuid(), "test", "transaction func", 1, new("https://func.com"), new { transaction = "func" }, Status.Enabled)
        );

        await entity.Update(Guid.NewGuid(), "rollback", "rollback", 2, new("https://rollback.com"), new { rollback = "rollback" }, Status.Disabled);

        throw new Exception();
    }
}