export class M3Config {
    apiUrl: string;
}

export class M3Request {
    program: string;
    transaction: string;
    param?: any;
    output?: string[];
    outputAll?: boolean;
    filter?: any;
    sort?: string[];
    orderByDesc?: boolean;
}