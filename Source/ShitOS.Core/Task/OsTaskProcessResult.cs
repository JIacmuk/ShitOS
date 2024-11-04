using ShitOS.Core.Task;

namespace ShitOS.Core;

public record struct OsTaskProcessResult(
    int RemainingTics,
    bool StateChanged
);