interface CategoryModel {
    id: string;
    name: string;
    logo: string;
}

interface PersonModel {
    id: string;
    name: string;
}

interface TransactionModel {
    id: string;
    name: string;
    amount: number;
    categoryId: string;
    payerId: string;
    sharedIds: string; // Id de pessoas
}