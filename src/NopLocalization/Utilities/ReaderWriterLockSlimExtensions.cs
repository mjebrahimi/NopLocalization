using System;
using System.Threading;

namespace AspNetCore.Localization
{
    /// <summary>
    /// Reader writer locking extensions
    /// </summary>
    public static class ReaderWriterLockSlimExtensions
    {
        /// <summary>
        /// Tries to enter the lock in read mode, with an optional integer time-out.
        /// </summary>
        public static void TryReadLocked(this ReaderWriterLockSlim readerWriterLock, Action action, int millisecondsTimeout = Timeout.Infinite)
        {
            if (!readerWriterLock.TryEnterReadLock(millisecondsTimeout))
                throw new TimeoutException();

            try
            {
                action();
            }
            finally
            {
                readerWriterLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Tries to enter the lock in write mode, with an optional time-out.
        /// </summary>
        public static void TryWriteLocked(this ReaderWriterLockSlim readerWriterLock, Action action, int millisecondsTimeout = Timeout.Infinite)
        {
            if (!readerWriterLock.TryEnterWriteLock(millisecondsTimeout))
                throw new TimeoutException();

            try
            {
                action();
            }
            finally
            {
                readerWriterLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Tries to enter the lock in upgradeable read mode, with an optional time-out.
        /// </summary>
        public static void TryUpgradeableReadLock(this ReaderWriterLockSlim readerWriterLock, Action action, int millisecondsTimeout = Timeout.Infinite)
        {
            if (!readerWriterLock.TryEnterUpgradeableReadLock(millisecondsTimeout))
                throw new TimeoutException();

            try
            {
                action();
            }
            finally
            {
                readerWriterLock.ExitUpgradeableReadLock();
            }
        }
    }
}
