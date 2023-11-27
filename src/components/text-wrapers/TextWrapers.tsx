import { ReactNode } from "react";

interface TextProps {
    children: string|string[];
    additionalClasses?: string[];
  }

function HeadingLarge({children, additionalClasses = []}: TextProps) {
    return(
        <h1 className={`heading--large ${additionalClasses.join(' ')}`}>
            {children}
        </h1>
    )
}

function HeadingSmall({children, additionalClasses = []}: TextProps) {
    return(
        <h3 className={`heading--small ${additionalClasses.join(' ')}`}>
            {children}
        </h3>
    )
}

function BodyNormal({children, additionalClasses = []}: TextProps) {
    return(
        <p className={`body--normal ${additionalClasses.join(' ')}`}>
            {children}
        </p>
    )
}

function BodySmall({children, additionalClasses = []}: TextProps) {
    return(
        <p className={`body--small ${additionalClasses.join(' ')}`}>
            {children}
        </p>
    )
}

function ButtonsLarge({children, additionalClasses = []}: TextProps) {
    return(
        <p className={`buttons--large ${additionalClasses.join(' ')}`}>
            {children}
        </p>
    )
}

function ButtonsNormal({children, additionalClasses = []}: TextProps) {
    return(
        <p className={`buttons--normal ${additionalClasses.join(' ')}`}>
            {children}
        </p>
    )
}

export {HeadingLarge, HeadingSmall, BodyNormal, BodySmall, ButtonsLarge, ButtonsNormal}