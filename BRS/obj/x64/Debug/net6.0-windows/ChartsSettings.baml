   M S B A M L   `   `   `  ����E  ACharts.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE1clr-namespace:Charts.WPF.Core;assembly=Charts.WPFCharts.WPF.Core  5      W:clr-namespace:Charts.WPF.ChartControls;assembly=Charts.WPFCharts.WPF.ChartControls  5      ]=clr-namespace:Charts.WPF.Core.ColumnChart;assembly=Charts.WPFCharts.WPF.Core.ColumnChart  5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   BackgroundBrush  �� Color  ForegroundBrush  PrimaryForeground  BasicTextStyle �� 
TargetType �� Property �� Value  BaselineTextStyle �� BasedOn  HeaderTextStyle T� X T� Y  PageHeaderTextStyle  SectionHeaderTextStyle  SeriesHeader 	 
ChartStyle    Charts.WPF.Core.ChartBase;   5Charts.WPF.ChartControls.ResourceDictionaryCollection 
 CustomPalette  Key  ListBoxItemStyle  PageContent �� CornerRadius �� Opacity	 �Name
 � RowDefinitions � ColumnDefinitions �� Orientation6   0Charts.WPF.Core.ColumnChart.ClusteredColumnChart    
ChartTitle    ChartSubTitle    Palette    SelectedItem �� Path �� Mode    SeriesSource    SeriesTemplate*   $Charts.WPF.ChartControls.ChartSeries   DisplayMember   ValueMember   SeriesTitle�� 5      H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      :x,http://schemas.microsoft.com/winfx/2006/xaml     5      :core1clr-namespace:Charts.WPF.Core;assembly=Charts.WPF  5      LchartControls:clr-namespace:Charts.WPF.ChartControls;assembly=Charts.WPF  5      McolumnChart=clr-namespace:Charts.WPF.Core.ColumnChart;assembly=Charts.WPF  5      %�  &	        &	 %     &	 H     &	 i     &	     7  &	 u    7  &	 �    7  &	 _    7  &	 �    7  &		 u    &	
 b    &	 �    &	     7   7 	 7 
 �� 5      $  Transparent��6.   �� 5	      $  	#FF666666��6.   ��5
      	#FF666666��61   �� 5      " ��6#   .�5   
   ��  � �� �	#FF6666666'   �� 5   
    � ��$ 20(�6%   �� 5   
    � ��$ Segoe UI)�6'   �� 5   
    � ��$ WordEllipsis=�6)   �� 5   
    � ��$
 Wrap=�6)   �� 5   
    �@K�StylisticSet20$
 True��66   �� 5   
    �@K�DiscretionaryLigatures$
 True��6>   �� 5   
    �@K�CaseSensitiveForms$
 True��6:   5      �� 5      " ��6&   8   .�5   
   ��  �@��LineStackingStrategy$ BlockLineHeight=�61   5      �� 5      " ��6$   8   .�5   
   ��  � ��$ 42(�6%   �� 5   
    � ��$ Light"�6'   �� 5   
    � ��$ WordEllipsis=�6)   �� 5   
    � ��$ NoWrap=�6)   �� 5    
    � ��$	 Top=�6.   �� 5!   
    � z� 5"      T� 5#      $ -1Q�6%   $ 0Q�6,   5%      5&      �� 5(      " ��6(   8   .�5)   
   ��  � ��$ 0,5,10,0q�6#   �� 5*   
    � ��$ 40(�6%   �� 5+   
    �@��
LineHeight$ 40��6'   5,      �� 5-      " ��6+   8   .�5.   
   ��  � ��$ 	0,0,10,10q�6#   �� 5/   
    � ��$ 26(�6%   50      �� 51      " ��6!   8   .�52   
   ��  � ��$ 	0,0,10,10q�6#   �� 53   
    � ��$
 Bold"�6'   �� 54   
    � ��$ 18(�6%   55      �� 58      "   6   .�59   
   ��  � ��$ 0 0 0 0q�6#   �� 5:   
    � ��
 ���� 6'   �� 5;   
    � �� �	#FF6666666'   �� 5<   
    � ��$ Segoe UI)�6'   �� 5=   
    � ��$ 11(�6%   5>        5@      �� 5A   
   �� 5B      
Brush1 $  	#FF6685A2��6-   5C      �� 5D   
   �� 5E      
Brush2 $  	#FF1D97E3��6-   5F      �� 5G   
   �� 5H      
Brush3 $  	#FFFF2900��6-   5I      �� 5J   
   �� 5K      
Brush4 $  	#FFFF8000��6-   5L      �� 5M   
   �� 5N      
Brush4 $
  Gray��6-   5O      5P      �� 5R      " ��6%   .�5S   
   ��  � ��$ Stretch=�67   5T      �� 5W      .R�5X   
   �� 
������ $��1,1,1,1q�6+   $ 20,20,20,20��6E   ��5Y      �� 5Z      $  White��6"   $	 0.8Q�60   ._�5\      � $	 maingrid��6   8��  
 5]      �� 5^      $��3* �6$   �� 5_      $��2* �6$    5a      �� 5b      $��1* �6'   �� 5c      $��8* �6'   �� 5d      $��1* �6'   .+�5f      �� $ 
Horizontal=�6   $��1��66   $��0��6F   .+�5g      �� $	��350��6!   .+�5h      �  5i      �� 5j   "   $
��auto �63   �� 5k   "   $
��auto �63   
 5m      �� 5n   "   $
��auto �60   �� 5o   "   $
��auto �60   .+�5r        $��0��5s   !   $��0��5t   !   8�� $ Trainingsgemiddelde��5v   !   $2 ,Hoeveel trainingen je per maand gedaan hebt.��5w   !   8   5y   !   �� $ SelectedItem�$ TwoWay=� 5z   !   �� *Series+ 5{   "   �� 5|   &   .R�5}   *     $ Category��5   -   $ Number��5�   -    5~   $   �� $ DisplayName���5�   -   �� $ Items�5�   '   5�      5�      5�      5�      5�      5�      5�      5�      