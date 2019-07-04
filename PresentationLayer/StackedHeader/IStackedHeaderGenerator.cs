using System.Windows.Forms;

namespace PresentationLayer
{
    public interface IStackedHeaderGenerator
    {
        Header GenerateStackedHeader(DataGridView objGridView);
    }
}
