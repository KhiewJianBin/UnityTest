public static class Lines
{
    public enum Algorithm
    {
        /// <summary>
        /// Bresenham's line algorithm.
        /// </summary>
        BRESENHAM,

        /// <summary>
        /// DDA line algorithm -- effectively an optimized algorithm for producing Brensham-like
        /// lines. There may be slight differences in appearance when compared to lines created
        /// with Bresenham's, however this algorithm may also be measurably faster. Based on the
        /// algorithm here:
        /// https://hbfs.wordpress.com/2009/07/28/faster-than-bresenhams-algorithm/, as well as
        /// the Java library Squidlib's implementation.
        /// </summary>
        DDA,
    }
}