﻿using System.Collections.Generic;
using CJBCheatsMenu.Framework.Menu;

namespace CJBCheatsMenu.Framework
{
    /// <summary>
    /// The manager that maintains the menus added to the main dialog menu.
    /// </summary>
    public class MenuManager : Menu.IMenuManager
    {
        /// <summary>
        /// A dictionary of registered menus.
        /// </summary>
        private Dictionary<string, Menu.IMenu> registeredMenus = new Dictionary<string, IMenu>();

        /// <summary>
        /// The order in which the menu tabs are displayed in the main dialog menu.
        /// </summary>
        private List<Menu.IMenu> orderedMenus = new List<IMenu>();

        /// <summary>
        /// The menus in there display order.
        /// </summary>
        public IReadOnlyCollection<Menu.IMenu> Menus
        {
            get
            {
                return orderedMenus.AsReadOnly();
            }
        }

        /// <summary>
        /// Remove a menu from the list of menus in the main dialog menu.
        /// </summary>
        /// <param name="menuId">The menuId of the menu to remove.</param>
        /// <returns>true if the menu was successfully removed, false otherwise.</returns>
        public bool DeregisterMenu(string menuId)
        {
            if (registeredMenus.ContainsKey(menuId))
            {
                orderedMenus.Remove(registeredMenus[menuId]);
                registeredMenus.Remove(menuId);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Adds a menu to the list of menus in the main dialog menu.
        /// </summary>
        /// <param name="menu">The menu to add.</param>
        /// <returns>true if the menu was successfully added, false otherwise.</returns>
        public bool RegisterMenu(IMenu menu)
        {
            if (registeredMenus.ContainsKey(menu.Id))
            {
                return false;
            }
            registeredMenus.Add(menu.Id, menu);
            orderedMenus.Add(menu);
            return true;
        }
    }
}
