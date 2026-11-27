using System.Threading;
using Backend.Fx.Util;
using Xunit;

namespace Backend.Fx.Tests.Util;

public class ReaderWriterLockSlimExtensionsTests
{
    [Fact]
    public void ReadAcquiresAndReleasesReadLock()
    {
        var sync = new ReaderWriterLockSlim();

        using (sync.Read())
        {
            Assert.True(sync.IsReadLockHeld);
        }

        Assert.False(sync.IsReadLockHeld);
    }

    [Fact]
    public void WriteAcquiresAndReleasesWriteLock()
    {
        var sync = new ReaderWriterLockSlim();

        using (sync.Write())
        {
            Assert.True(sync.IsWriteLockHeld);
        }

        Assert.False(sync.IsWriteLockHeld);
    }

    [Fact]
    public void LockCanBeReacquiredAfterRelease()
    {
        var sync = new ReaderWriterLockSlim();

        using (sync.Write()) { }

        using (sync.Read())
        {
            Assert.True(sync.IsReadLockHeld);
        }
    }
}
