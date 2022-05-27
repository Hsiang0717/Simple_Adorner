using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;


namespace ResizingAdomer
{
    /// <summary>
    /// 크기 조정 어도너
    /// </summary>
    public class ResizingAdorner : Adorner
    {
   
        private double initialAngle;
        private double angle;
        private RotateTransform rotateTransform;
        private Vector startVector;
        private Point transformOrigin;
        private Point centerPoint;
        private InkCanvas canvas;


        //////////////////////////////////////////////////////////////////////////////////////////////////// Field
        ////////////////////////////////////////////////////////////////////////////////////////// Private

        #region Field

        /// <summary>
        /// 좌상단 썸
        /// </summary>
        private Thumb topLeftThumb;
        
        /// <summary>
        /// 우상단 썸
        /// </summary>
        private Thumb topRightThumb;
        
        /// <summary>
        /// 좌하단 썸
        /// </summary>
        private Thumb bottomLeftThumb;
        
        /// <summary>
        /// 우하단 썸
        /// </summary>
        private Thumb bottomRightThumb;

        /// <summary>
        /// 비주얼 컬렉션
        /// </summary>
        private VisualCollection visualCollection;

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Property
        ////////////////////////////////////////////////////////////////////////////////////////// Protected

        #region 비주얼 자식 수 - VisualChildrenCount

        /// <summary>
        /// 비주얼 자식 수
        /// </summary>
        protected override int VisualChildrenCount
        {
            get
            {
                return this.visualCollection.Count;
            }
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Constructor
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 생성자 - ResizingAdorner(targetElement)

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="targetElement">타겟 엘리먼트</param>
        public ResizingAdorner(UIElement targetElement) : base(targetElement)
        {
            this.visualCollection = new VisualCollection(this);
        
            SetThumb(ref this.topLeftThumb    , Cursors.ScrollAll, "TopLeft");
            SetThumb(ref this.topRightThumb   , Cursors.Arrow, "TopRight");
            SetThumb(ref this.bottomLeftThumb , Cursors.Arrow, "BottomLeft");
            SetThumb(ref this.bottomRightThumb, Cursors.SizeNWSE, "BottomRight");

            this.topLeftThumb.DragDelta     += topLeftThumb_DragDelta;
            this.topRightThumb.DragDelta    += topRightThumb_DragDelta;
            this.bottomLeftThumb.DragDelta  += bottomLeftThumb_DragDelta;
            this.bottomRightThumb.DragDelta += bottomRightThumb_DragDelta;

            this.topRightThumb.DragStarted += topRightThumb_DragStarted;
            this.topLeftThumb.DragStarted += topLeftThumb_DragStarted;
            this.bottomLeftThumb.DragStarted += bottomLeftThumb_DragStarted;
            this.bottomRightThumb.DragStarted += bottomRightThumb_DragStarted;


        }
        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Protected
        //////////////////////////////////////////////////////////////////////////////// Function

        #region 비주얼 자식 수 구하기 - GetVisualChild(index)

        /// <summary>
        /// 비주얼 자식 수 구하기
        /// </summary>
        /// <param name="index">인덱스</param>
        /// <returns>비주얼 자식 수</returns>
        protected override Visual GetVisualChild(int index)
        {
            return this.visualCollection[index];
        }

        #endregion
        #region 배열하기 (오버라이드) - ArrangeOverride(finalSize)

        /// <summary>
        /// 배열하기 (오버라이드)
        /// </summary>
        /// <param name="finalSize">최종 크기</param>
        /// <returns>크기</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            double desiredWidth  = AdornedElement.DesiredSize.Width;
            double desiredHeight = AdornedElement.DesiredSize.Height;
            
            double adornerWidth  = DesiredSize.Width;
            double adornerHeight = DesiredSize.Height;



            this.topLeftThumb.Arrange
                       (
                           new Rect
                           (
                               -adornerWidth / 2 +30,
                               -adornerHeight / 2 +30,
                               adornerWidth,
                               adornerHeight
                           )
                       );

            this.topRightThumb.Arrange
            (
                new Rect
                (
                    desiredWidth - adornerWidth / 2 -30,
                    -adornerHeight / 2 +30,
                    adornerWidth,
                    adornerHeight
                )
            );

            this.bottomLeftThumb.Arrange
            (
                new Rect
                (
                    -adornerWidth / 2 +30,
                    desiredHeight - adornerHeight / 2 -30,
                    adornerWidth,
                    adornerHeight
                )
            );

            this.bottomRightThumb.Arrange
            (
                new Rect
                (
                    desiredWidth - adornerWidth / 2 -30,
                    desiredHeight - adornerHeight / 2 -30,
                    adornerWidth,
                    adornerHeight
                )
            );

            return finalSize;
        }

        #endregion

        ////////////////////////////////////////////////////////////////////////////////////////// Private
        //////////////////////////////////////////////////////////////////////////////// Event

        #region 좌상단 썸 드래그 델타 처리하기 - topLeftThumb_DragDelta(sender, e)

        /// <summary>
        /// 좌상단 썸 드래그 델타 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void topLeftThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            FrameworkElement element = AdornedElement as FrameworkElement;

            Thumb thumb = sender as Thumb;

            if (element == null || thumb == null)
            {
                return;
            }

            this.rotateTransform = element.RenderTransform as RotateTransform;
        }
        private void topLeftThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            FrameworkElement element = AdornedElement as FrameworkElement;

            Thumb thumb = sender as Thumb;

            if(element == null || thumb == null)
            {
                return;
            }


            fp_Move_Control(sender, e, element, element.Margin.Left, element.Margin.Top);
            //SetSize(element);
            
            //element.Width  = Math.Max(element.Width  - e.HorizontalChange, thumb.DesiredSize.Width );
            //element.Height = Math.Max(element.Height - e.VerticalChange  , thumb.DesiredSize.Height);

        }

        #endregion
        #region 우상단 썸 드래그 델타 처리하기 - topRightThumb_DragDelta(sender, e)

        /// <summary>
        /// 우상단 썸 드래그 델타 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        private void topRightThumb_DragStarted(object sender, DragStartedEventArgs e)
        {

            FrameworkElement element = AdornedElement as FrameworkElement;

            Thumb thumb = sender as Thumb;

            if (element == null || thumb == null)
            {
                return;
            }
            canvas = element.Parent as InkCanvas;
            Console.WriteLine(canvas);

            if (canvas != null)
            {
                canvas.Children.Remove(element);
                canvas = null;
                element = null;
            }


            /*if (this.rotateTransform != null)
            {
                this.angle = this.rotateTransform.Angle * Math.PI / 180.0;
            }
            else
            {
                this.angle = 0.0d;
            }*/

        }
        private void topRightThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            FrameworkElement element = AdornedElement as FrameworkElement;

            Thumb thumb = sender as Thumb;

            if (element == null || thumb == null)
            {
                return;
            }

        }
        #endregion
        #region 좌하단 썸 드래그 델타 처리하기 - bottomLeftThumb_DragDelta(sender, e)

        /// <summary>
        /// 좌하단 썸 드래그 델타 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>
        /// 
        private void bottomLeftThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            FrameworkElement element = AdornedElement as FrameworkElement;

            Thumb thumb = sender as Thumb;

            if (element == null || thumb == null)
            {
                return;
            }

            this.canvas = VisualTreeHelper.GetParent(element) as InkCanvas;

            this.centerPoint = element.TranslatePoint(
                    new Point(element.Width * element.RenderTransformOrigin.X,
                                element.Height * element.RenderTransformOrigin.Y),
                                this.canvas);

            Point startPoint = Mouse.GetPosition(this.canvas);

            this.startVector = Point.Subtract(startPoint, this.centerPoint);

            this.rotateTransform = element.RenderTransform as RotateTransform;
            if (rotateTransform == null)
            {
                element.RenderTransform = new RotateTransform(0);
                this.initialAngle = 0;
            }
            else
            {
                this.initialAngle = this.rotateTransform.Angle;
            }

        }
        private void bottomLeftThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            FrameworkElement element = AdornedElement as FrameworkElement;

            Thumb thumb = sender as Thumb;

            if (element == null || thumb == null)
            {
                return;
            }

            element.RenderTransformOrigin = new Point(0.5, 0.5);

            Point currentPoint = Mouse.GetPosition(this.canvas);
            Vector deltaVector = Point.Subtract(currentPoint, this.centerPoint);

            double angle = Vector.AngleBetween(this.startVector, deltaVector);

            RotateTransform rotateTransform = element.RenderTransform as RotateTransform;
            rotateTransform.Angle = this.initialAngle + Math.Round(angle, 0);
            element.InvalidateMeasure();
        
        }

        #endregion
        #region 우하단 썸 드래그 델타 처리하기 - bottomRightThumb_DragDelta(sender, e)

        /// <summary>
        /// 우하단 썸 드래그 델타 처리하기
        /// </summary>
        /// <param name="sender">이벤트 발생자</param>
        /// <param name="e">이벤트 인자</param>

        private void bottomRightThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            FrameworkElement element = AdornedElement as FrameworkElement;

            Thumb thumb = sender as Thumb;

            if (element == null || thumb == null)
            {
                return;
            }
            this.canvas = VisualTreeHelper.GetParent(element) as InkCanvas;
            this.transformOrigin = thumb.RenderTransformOrigin;
            this.rotateTransform = element.RenderTransform as RotateTransform;
        }
        private void bottomRightThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            FrameworkElement element = AdornedElement as FrameworkElement;

            Thumb thumb = sender as Thumb;

            if(element == null || thumb == null)
            {
                return;
            }

            //https://stackoverflow.com/questions/59803689/wpf-adorner-for-control-user-resizable

            double deltaHorizontal = Math.Min(-e.HorizontalChange, element.ActualWidth - element.MinWidth);
            double y1 = element.Margin.Top - transformOrigin.X * deltaHorizontal * Math.Sin(angle);
            double x1 = element.Margin.Left + (deltaHorizontal * transformOrigin.X * (1 - Math.Cos(angle)));
            element.Margin = new Thickness(x1, y1, -x1, -y1);
            element.Width -= deltaHorizontal;

            double deltaVertical = Math.Min(-e.VerticalChange, element.ActualHeight - element.MinHeight);
            double y2 = element.Margin.Top + (transformOrigin.Y * deltaVertical * (1 - Math.Cos(-angle)));
            double x2 = element.Margin.Left - deltaVertical * transformOrigin.Y * Math.Sin(-angle);

            element.Margin = new Thickness(x2, y2, -x2, -y2);

            element.Height -= deltaVertical;

            e.Handled = true;

            //SetSize(element);
            // fp_Move_Control(sender, e, element, element.Margin.Left, element.Margin.Top);
            //element.Width  = Math.Max(element.Width    + e.HorizontalChange, thumb.DesiredSize.Width );
            //element.Height = Math.Max(e.VerticalChange + element.Height    , thumb.DesiredSize.Height);
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////// Function

        #region 썸 설정하기 - SetThumb(thumb, cursor)

        /// <summary>
        /// 썸 설정하기
        /// </summary>
        /// <param name="thumb">코너 썸</param>
        /// <param name="cursor">커서</param>
        private void SetThumb(ref Thumb thumb, Cursor cursor, String style)
        {
            if(thumb != null)
            {
                return;
            }

            thumb = new Thumb();

            Thickness thickness = new Thickness(25);
            thumb.Height = 2 * Math.Abs(thickness.Top);
            thumb.Width   = 2 * Math.Abs(thickness.Left);
            thumb.Opacity    = 1;
            thumb.Style = Application.Current.FindResource(style) as Style;
            //thumb.Background = new SolidColorBrush(Colors.MediumBlue);
            // thumb.Background = Application.Current.FindResource("xxx") as SolidColorBrush;
            thumb.Cursor     = cursor;

            this.visualCollection.Add(thumb);
        }

        #endregion
        #region 크기 설정하기 - SetSize(targetElement)

        /// <summary>
        /// 크기 설정하기
        /// </summary>
        /// <param name="targetElement">타겟 엘리먼트</param>
        private void SetSize(FrameworkElement targetElement)
        {
            if(targetElement.Width.Equals(Double.NaN)) 
            {
                targetElement.Width = targetElement.DesiredSize.Width;
            }

            if(targetElement.Height.Equals(Double.NaN))
            {
                targetElement.Height = targetElement.DesiredSize.Height;
            }

            FrameworkElement parentElement = targetElement.Parent as FrameworkElement;

            if(parentElement != null)
            {
                targetElement.MaxHeight = parentElement.ActualHeight;
                targetElement.MaxWidth  = parentElement.ActualWidth;
            }
        }

        #endregion

        public void fp_Move_Control(object sender, DragDeltaEventArgs e, FrameworkElement targetElement, double x, double y)
        {

            //< Vertical >https://codedocu.de/Details?d=997&a=9&f=129&l=0

            Point dragDelta = new Point(e.HorizontalChange, e.VerticalChange);
            
            if (this.rotateTransform != null)
            {
                dragDelta = this.rotateTransform.Transform(dragDelta);
            }

            //targetElement.Margin = new Thickness(posX, posY, -posX, -posY);
            targetElement.Margin = new Thickness(x+ dragDelta.X, y+ dragDelta.Y, -(x+ dragDelta.X), -(y+ dragDelta.Y));
        }

    }
}