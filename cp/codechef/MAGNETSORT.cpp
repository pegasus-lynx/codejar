#include<bits/stdc++.h>
using namespace std;

#define int         long long
#define ull         unsigned long long
#define ll             long long
#define pb             push_back
#define all(a)      (a).begin(),(a).end()
#define sz(x)       (ll)x.size()
#define endl        '\n'
#define gcd(a,b)    __gcd((a),(b))
#define lcm(a,b)    ((a)*(b)) / gcd((a),(b))
#define ios            ios_base::sync_with_stdio(false);cin.tie(0);cout.tie(0);
#define mp             make_pair
#define lb             lower_bound
#define ub            upper_bound
#define F           first
#define S           second
#define rep(i, begin, end) for (__typeof(end) i = (begin) - ((begin) > (end)); i != (end) - ((begin) > (end)); i += 1 - 2 * ((begin) > (end)))
#define hell         (ull)1e9

signed main(void)
{
    ios
    freopen("input.txt", "r", stdin);freopen("output.txt", "w", stdout);

    int TESTS=1, N=1;
    cin>>TESTS;

    while(TESTS--){
        cin>>N;
        vector<int> arr(N);
        vector<int> arrs(N);
        vector<char> pol(N);

        rep(i, 0, N) { cin>>arr[i]; arrs[i]=arr[i]; }
        rep(i, 0, N) cin>>pol[i];

        sort(arrs.begin(),arrs.end());

        bool north=false, south=false;
        rep(i, 0, N)
        {
            if(pol[i]=='N') north = true;
            if(pol[i]=='S') south = true;
        }

        bool sorted=true;
        rep(i,0,N)
        {
            if(arrs[i]!=arr[i]) sorted=false;
        }
        
        if(sorted)
        {
            cout<<0<<"\n";
            continue;
        }

        if(north && south)
        {
            int p=0, q=N-1;
            while(arr[p]==arrs[p]) p++;
            while(arr[q]==arrs[q]) q--;

            north=false;
            south=false;
            bool twice=true;
            rep(i,0,p+1)
            {
                if(pol[i]=='N') north=true;
                if(pol[i]=='S') south=true;
            }
            rep(i,q,N)
            {
                if(pol[i]=='N' && south)
                {
                    cout<<1<<"\n";
                    twice=false;
                    break;
                }
                if(pol[i]=='S' && north)
                {
                    cout<<1<<"\n";
                    twice=false;
                    break;
                }
            }

            if(twice) cout<<2<<"\n";
        }
        else
        {
            cout<<-1<<"\n";
        }
    }
    return 0;
}