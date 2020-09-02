namespace StarSonata.Api.Extensions
{
    using System;
    using System.Threading;

    public static class ReaderWriterLockSlimExtensions
    {
        public static IDisposable ReadLock(this ReaderWriterLockSlim obj, bool upgradable = false)
        {
            if (upgradable)
            {
                return new LockToken(obj.EnterUpgradeableReadLock, obj.ExitUpgradeableReadLock);
            }

            return new LockToken(obj.EnterReadLock, obj.ExitReadLock);
        }

        public static IDisposable WriteLock(this ReaderWriterLockSlim obj)
        {
            return new LockToken(obj.EnterWriteLock, obj.ExitWriteLock);
        }

        private struct LockToken : IDisposable
        {
            private readonly Action release;
            private readonly LeakGuard leakGuard;

            public LockToken(Action aquire, Action release)
            {
                this.leakGuard = new LeakGuard();
                this.release = release;
                try
                {
                    aquire();
                }
                catch (ObjectDisposedException)
                {
                    // Ignored
                }
            }

            public void Dispose()
            {
                try
                {
                    this.release();
                }
                catch (ObjectDisposedException)
                {
                    // Ignored
                }

                GC.SuppressFinalize(this.leakGuard);
            }

            private class LeakGuard
            {
                ~LeakGuard()
                {
                    throw new InvalidOperationException("Lock not properly disposed.");
                }
            }
        }
    }
}
