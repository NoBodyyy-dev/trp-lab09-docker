using Lab07_RazorPages.Models;

namespace Lab07_RazorPages.Data;

public interface IPredictionRepository
{

    PagedResult<Prediction> GetPage(PredictionQuery query);

    Prediction? GetById(int id);

    Prediction Add(PredictionWriteDto dto);

    Prediction? Replace(int id, PredictionWriteDto dto);

    Prediction? Patch(int id, PredictionPatchDto dto);

    bool Delete(int id);

    Prediction? GetRandom(string? sign);

    int Count { get; }
}
