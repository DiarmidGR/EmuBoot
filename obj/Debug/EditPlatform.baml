   M S B A M L   `   `   `  ����B  >EmuBoot, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:EmuBootEmuBoot  5   	       EmuBoot.EditPlatform   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:EmuBoot     5   	     � Title$  EditPlatform��5   	   $	��450��6   $	��600��6+   ���#222E50 � 
ResizeMode$ NoResize=�6L    � WindowStartupLocation$ CenterOwner=�6b   .��5	      �  � RowDefinitions 5
   
   �� 5      �� 5      �� 5      �� 5      �� 5      �� 5       � ColumnDefinitions 5   
   �� 5      �� 5      .+�5   
   �� $��Platform name (ex: NDS): ��6   $��Center=�64   $��Right=�6O   $��10q�6k   $��0��6w   $��0��6�   $��18(�6�   �� 5   
   -   5         ��Name$ TextName��5      $	��200��6$   $��Center=�63   $
��Left=�6N   $��10q�6i   $��1��6u   $��0��6�   �� 5   
   $��Path to emulator .exe: ��6   $	��Top=�62   $��Right=�6J   $��10q�6f   $��0��6r   $��1��6�   $��18(�6�   �� 5   
   -   5         ��Name$ 	BrowseEmu��5      $	��100��6$   $��30��63   $	��Top=�6B   $
��Left=�6Z   $��10q�6u   $��1��6�   $��1��6�   $��Browse Files��6�   6�   �� 5   
   -   5         ��Name$ LabelEmu��5      $��(Current emulator path)��6"   $��Bottom=�6D   $��Center=�6_   $��0��6|   $��1��6�   $��2��6�   $��16(�6�   �� 5   
   $��Path to roms: ��6   $	��Top=�6)   $��Right=�6A   $��10q�6]   $��0��6i   $��2��6y   $��18(�6�   �� 5   
   -   5        $ 
BrowseRoms��5      $	��100��6%   $��30��64   $	��Top=�6C   $
��Left=�6[   $��10q�6v   $��1��6�   $��2��6�   $��Browse Files��6�   6�   �� 5    
   -   5        $ 	LabelRoms��5       $��(Current roms path)��6#   $��Bottom=�6A   $��Center=�6\   $��0��6y   $��2��6�   $��2��6�   $��16(�6�   �� 5"   
   $��Path to covers: ��6   $	��Top=�6+   $��Right=�6C   $��10q�6_   $��0��6k   $��3��6{   $��18(�6�   �� 5#   
   -   5        $ BrowseCovers��5#      $	��100��6'   $��30��66   $	��Top=�6E   $
��Left=�6]   $��10q�6x   $��1��6�   $��3��6�   $��Browse Files��6�   6�   �� 5$   
   -   5        $ LabelCovers��5$      $��(Current covers path)��6%   $��Bottom=�6E   $��Center=�6`   $��0��6}   $��3��6�   $��2��6�   $��16(�6�   �� 5&   
   $ ��Rom extension (ex: .nds): ��6   $��Center=�65   $��Right=�6P   $��10q�6l   $��0��6x   $��4��6�   $��18(�6�   �� 5'   
   -   5        $ TextExtension��5'      $	��200��6)   $��Center=�68   $
��Left=�6S   $��10q�6n   $��1��6z   $��4��6�   �� 5)   
   -	   5        $ SaveEdit��5)      $	��100��6#   $��30��62   $��Center=�6A   $��Center=�6\   $��5��6y   $��2��6�   $��	Save Edit��6�   6�   5*      5+      