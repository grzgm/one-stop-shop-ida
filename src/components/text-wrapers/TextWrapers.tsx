import { ReactNode } from "react";

interface TextProps {
    children: string;
  }

function HeadingLarge({children}: TextProps) {
    return(
        <h1 className="heading--large">
            {children}
        </h1>
    )
}

function HeadingSmall({children}: TextProps) {
    return(
        <h3 className="heading--small">
            {children}
        </h3>
    )
}

function BodyNormal({children}: TextProps) {
    return(
        <p className="body--normal">
            {children}
        </p>
    )
}

function BodySmall({children}: TextProps) {
    return(
        <p className="body--small">
            {children}
        </p>
    )
}

function ButtonsLarge({children}: TextProps) {
    return(
        <p className="buttons--large">
            {children}
        </p>
    )
}

function ButtonsNormal({children}: TextProps) {
    return(
        <p className="buttons--normal">
            {children}
        </p>
    )
}

export {HeadingLarge, HeadingSmall, BodyNormal, BodySmall, ButtonsLarge, ButtonsNormal}