   M S B A M L   `   `   `  ����C  ?FBDMaker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:FBDMakerFBDMaker  5   	       FBDMaker.GenreTree   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   $lclr-namespace:FBDMaker     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   +   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6h   -   5          � Title$  Тематики��5   	   $	��368��6�   $	��343��6�    � 
ResizeMode$ NoResize=�6�    � WindowStartupLocation$ CenterScreen=�6�    � WindowStyle$ 
ToolWindow=�6   � 	Resources 5      !   FBDMaker.WPFConvCheckVisibl   WPFConvCheckVisibl1  tr1 �� DataType   FBDMaker.GenreNode �� ItemsSource �� Orientation d� 	IsChecked	 �� Path
 �� 	Converter��%  &	        &	      7     5   
   6   �� 5	   
   "  6/    5
   %   �� *Chaild+.R�5      �� $ 
Horizontal=�6   .+�5      ��  6   �� *
IsCheckVis+y�6<   �� $	 IsGenre�8
   �� 5      ��6   �� *Name+5      5      5      .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   R� -   5        $��	treeView1��5       $��0��6   #���  61   ��5      �� 5       �� 
TargetType" Q�6   .�5      ��  �� Property �@Q�
IsExpanded �� Value 63   �� *Expand+ �� Mode$ TwoWay=�5       5#      � 5$   
   $��1��6    � ColumnDefinitions 5%      �� 5&      $��* �6#   �� 5'      $��* �6#   .+�5)      �� -   5        $��Cancel��5)   %   $��0��6   $��Отмена��63    �� IsCancel . 6E   �� 5*      -   5        $��Ok��5*   %   $��1��6   $��Принять��6/    �� 	IsDefault . 6R   5+      5,      5-      