using ISpy.ViewModels;
using Microsoft.Maui.Layouts;

namespace ISpy.Views;

public partial class GamePage : ContentPage
{
    private readonly GameViewModel _viewModel;
    private readonly Dictionary<SceneItemViewModel, View> _sceneViews = new();

    public GamePage()
    {
        InitializeComponent();
        _viewModel = new GameViewModel();
        BindingContext = _viewModel;
        _viewModel.Scene.CollectionChanged += (_, _) => RebuildScene();
    }

    private void OnSceneSizeChanged(object? sender, EventArgs e) => RebuildScene();

    private void RebuildScene()
    {
        SceneLayout.Children.Clear();
        _sceneViews.Clear();

        double canvasW = SceneLayout.Width;
        double canvasH = SceneLayout.Height;
        if (canvasW <= 0 || canvasH <= 0)
            return;

        foreach (var item in _viewModel.Scene)
        {
            var image = new Image
            {
                Source = item.ImageFile,
                Aspect = Aspect.AspectFit,
                Opacity = item.Opacity,
                BindingContext = item,
            };
            image.SetBinding(Image.OpacityProperty, nameof(SceneItemViewModel.Opacity));

            var tap = new TapGestureRecognizer();
            tap.Tapped += (_, _) => _viewModel.TapItem(item);
            image.GestureRecognizers.Add(tap);

            double x = item.X * canvasW;
            double y = item.Y * canvasH;
            double w = item.W * canvasW;
            double h = item.H * canvasH;

            var bounds = new Rect(x, y, w, h);
            AbsoluteLayout.SetLayoutBounds(image, bounds);
            AbsoluteLayout.SetLayoutFlags(image, AbsoluteLayoutFlags.None);
            image.Rotation = item.Rotation;

            SceneLayout.Children.Add(image);
            _sceneViews[item] = image;
        }
    }
}
