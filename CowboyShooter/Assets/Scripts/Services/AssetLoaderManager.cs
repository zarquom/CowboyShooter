using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IAssetLoader
{
    // Define methods for loading assets
    Task PreloadLabelAsync(string label);
    T GetAsset<T>(string key) where T : UnityEngine.Object;
    void ReleaseAllAssets();
    bool IsLabelReady(string label);
}
public class AssetLoaderManager : IAssetLoader
{
    private readonly Dictionary<string, AsyncOperationHandle<UnityEngine.Object>> assetHandles =
        new Dictionary<string, AsyncOperationHandle<UnityEngine.Object>>();

    private readonly HashSet<string> loadedLabels = new HashSet<string>();

    public async Task PreloadLabelAsync(string label)
    {
        if (loadedLabels.Contains(label))
            return;

        // Resolve every location under this label first, regardless of asset type.
        var locationsHandle = Addressables.LoadResourceLocationsAsync(label);
        IList<IResourceLocation> locations = await locationsHandle.Task;

        var loadTasks = new List<Task>();
        foreach (var location in locations)
        {
            if (assetHandles.ContainsKey(location.PrimaryKey))
                continue; // already loaded (e.g. shared with another label)

            var handle = Addressables.LoadAssetAsync<UnityEngine.Object>(location);
            assetHandles[location.PrimaryKey] = handle;
            loadTasks.Add(handle.Task);
        }

        await Task.WhenAll(loadTasks);
        Addressables.Release(locationsHandle);

        foreach (var location in locations)
        {
            if (assetHandles.TryGetValue(location.PrimaryKey, out var h) && h.Status != AsyncOperationStatus.Succeeded)
                Debug.LogError($"Failed to load asset '{location.PrimaryKey}' from label '{label}'");
        }
        Debug.Log($"[AssetLoaderManager] PreloadLabelAsync {label} assets loaded");
        loadedLabels.Add(label);
    }
    public bool IsLabelReady(string label) => loadedLabels.Contains(label);

    // Fetch any preloaded asset by its Addressable address, cast to the type you need.
    public T GetAsset<T>(string key) where T : UnityEngine.Object
    {
        if (!assetHandles.TryGetValue(key, out var handle) || handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogWarning($"Asset '{key}' is not loaded.");
            return null;
        }

        if (handle.Result is T typed)
            return typed;

        Debug.LogWarning($"Asset '{key}' is loaded but is a {handle.Result.GetType().Name}, not {typeof(T).Name}.");
        return null;
    }


    public void ReleaseAllAssets()
    {
        foreach (var handle in assetHandles.Values)
        {
            if (handle.IsValid())
                Addressables.Release(handle);
        }
        assetHandles.Clear();
        loadedLabels.Clear();
    }
}
