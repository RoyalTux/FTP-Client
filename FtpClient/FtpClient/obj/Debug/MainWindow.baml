   M S B A M L   `   `   `  ����D  @FtpClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:FtpClient	FtpClient  5   	       FtpClient.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:FtpClient     5   	   -   5           MaterialDesignBody#���   5   	   
���   �  � Title$  
FTP Client��5
   	   $��441.6��5   "   .��5      �  � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      .+�5   
   �� $��0,0,1,0q�6   
�������$��5q�5      ._�5      � $	��300��6    5      �� 5      $
��0.4* �6'   �� 5      $
��0.6* �6'    � RowDefinitions 5      �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   .+�5"      �� $	��IP:��6   �� 5#      -   5         ��Name$ txtboxAddress��5#      $��1��5$      $��ftp://��5%      �� 5&      $��Login:��6   $��1��5'      �� 5(      -   5        $ txtboxLogin��5(      $��1��5)      $��1��5*      �� 5+      $��	Password:��6   $��2��5,      G� 5-      -   5         G�Name$ txtboxPassword��5-      $��2��5.      $��1��5/      �� 50      -   5         ��Name$ btn_connect��50      
��� ���$��Connect FTP��52      $��0,10.2,0,10.6q�53      $��3��54      $��2��55      $	��300��56      $
��Left=�57      58      �� 59      -   5        $ download��59      
�������$��Download file��5;      $��0,85.2,0,-61.2q�5<      $��3��5=      $��2��5>      5?      �� 5A      -   5        $ upload��5B      
���  ��$��Upload file��5D      $��0,123.2,0,-97.6q�5E      $��3��5F      $��2��5G      5H      5J      5K      �� 5L   
   -   5         ��Name$ listbox��5N      $��1��5L      $��5q�5M      $��Stretch=�5O      5P      5Q      