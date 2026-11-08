#include<bits/stdc++.h> 
using namespace std;

#define int         long long
#define ull         unsigned long long
#define ll          long long
#define pb          push_back
#define all(a)      (a).begin(),(a).end()
#define sz(x)       (ll)x.size()
#define endl        '\n'
#define gcd(a,b)    __gcd((a),(b))
#define lcm(a,b)    ((a)*(b)) / gcd((a),(b))
#define ios         ios_base::sync_with_stdio(false);cin.tie(0);cout.tie(0);
#define mp          make_pair
#define lb          lower_bound
#define ub          upper_bound
#define F           first
#define S           second
#define rep(i, begin, end) for (__typeof(end) i = (begin) - ((begin) > (end)); i != (end) - ((begin) > (end)); i += 1 - 2 * ((begin) > (end)))
#define hell        (ull)1e9

void solve();
int bsstring(int start, int end, int size);

signed main(void)
{
    ios
    freopen("input.txt", "r", stdin);freopen("output.txt", "w", stdout);
    int TESTS=1;
    cin>>TESTS;

    while(TESTS--){
        solve();
    }
    return 0;
}

void solve()
{
    int n, m;
    cin >> n >> m;

    int mar = (((n+2)*(n+1))/2)-1;
    int mir = bsstring(0,n-1,n);

    if(m<mir)
    {
        cout << -1 << "\n";
    }
    else if(m>mar)
    {
        cout << m-mar << "\n";
    }
    else
    {
        cout << 0 << "\n";
    }
}

int bsstring(int start, int end, int size)
{
    if(start > end) return 0;

    int mid = (start+end)/2;
    return bsstring(mid+1,end,size) + bsstring(start,mid-1,size) + (1+end-mid) + (1+mid-start);
}