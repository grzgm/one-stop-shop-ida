import { createContext, useState } from 'react';

const [showSidebar, setShowSidebar] = useState(false)

export const ShowSidebarContext = createContext({showSidebar: showSidebar, switchShowSidebar: setShowSidebar})